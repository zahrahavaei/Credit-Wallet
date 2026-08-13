using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Exceptions;
using Credit_Wallet.Repositories;
using Microsoft.EntityFrameworkCore;
using Credit_Wallet.Services;


namespace Credit_Wallet.Features.AddCreditToWallet
{
    public class AddCreditToWalletHandler
    {
        
        private readonly AddCreditToWalletValidator _validator;
        private readonly ILogger<AddCreditToWalletHandler> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly HmacService _hmacService;
        private readonly WalletIntegrityService _walletIntegrityService;
        public AddCreditToWalletHandler( AddCreditToWalletValidator validator,
                                        ILogger<AddCreditToWalletHandler> logger,
                                        IServiceScopeFactory serviceScopeFactory,
                                        HmacService hmacService,
                                        WalletIntegrityService walletIntegrityService)
        {
            _validator = validator;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _hmacService = hmacService;
            _walletIntegrityService = walletIntegrityService;
        }
        private async Task PerformAddAsync(Wallet wallet,
                                                    decimal amount,
                                                   ITransactionRepository transactionRepository,
                                                   IUnitOfWork unitOfWork)
        {
            var createDate = DateTimeHelper.NormalizeToMilliseconds(DateTime.UtcNow);
            var transactionData = $"{wallet.Id}|{amount}|{TransactionType.Deposit}|{createDate}";
            var transactionHash = _hmacService.GenerateHmacHash(transactionData);
            await  transactionRepository.AddTransactionAsync(new Transaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                TransactionType = TransactionType.Deposit,
                CreatedDateTime = createDate,
                TransactionHash = transactionHash
            });
            wallet.Balance += amount;
            wallet.LastUpdateDateTime = DateTimeHelper.NormalizeToMilliseconds(DateTime.UtcNow);
            wallet.RowVersion = Guid.NewGuid();
            wallet.WalletHash = _walletIntegrityService.GenerateWalletHash(wallet);
            await unitOfWork.SaveChangesAsync();
        }
        public async Task<AddCredittoWalletResponse> HandleAsync(AddCreditToWalletRequest request)
        {
            if (!_validator.Validate(request))
            {
                 return new AddCredittoWalletResponse
                {
                    Success = false,
                    Message = "Invalid request ",
                   
                };
            }
            const int maxAttempt= 2;
            for (var attempt = 1; attempt <= maxAttempt; attempt++)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var walletRepository = scope.ServiceProvider.GetRequiredService<IWalletRepository>();
                    var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var wallet = await walletRepository.GetWalletByUserIdAsync(request.UserId);

                    if (wallet == null)
                    {
                        return new AddCredittoWalletResponse
                        {
                            Success = false,
                            Message = $"Wallet not found for this {request.UserId}",

                        };
                    }
                    if (!_walletIntegrityService.VerifyWallet(wallet))
                    {
                        _logger.LogError($"Wallet integrity check failed for wallet ID: {wallet.Id},userId{wallet.UserId}"  ,wallet.Id, request.UserId);
                        return new AddCredittoWalletResponse
                        {
                            Success = false,
                            Message = "Unable to process the request!",
                        };
                    }
                    await PerformAddAsync(wallet, request.Amount, transactionRepository, unitOfWork);
                    return new AddCredittoWalletResponse
                    {
                        Success = true,
                        Message = "Credit added successfully",
                        NewBalance = wallet.Balance
                    };
                }
                catch(WalletConcurrencyException ex)
                {
                    _logger.LogWarning(ex, "The Wallet Is Modified By Another Request,{UserId}", request.UserId);
                  
                    if (attempt == maxAttempt)
                    {
                        _logger.LogError("Failed to deduct amount for UserId: {UserId} after 2 attempts due to concurrency issues.",
                                         request.UserId);
                        return new AddCredittoWalletResponse
                        {
                            Success = false,
                            Message = "Failed to deduct amount . Please try again.",
                        };
                    }
                }
                catch (DatabaseException ex)
                {
                    _logger.LogError(ex,
                       "Database update error while deducting amount for UserId: {UserId}", request.UserId);
                    return new AddCredittoWalletResponse
                    {
                        Success = false,
                        Message = "An error occurred while processing your request.",
                    };
                }
              
            }
            return new AddCredittoWalletResponse
            {
                Success = false,
                Message = "Unable to process request. .",
            };

        }
    }
}
