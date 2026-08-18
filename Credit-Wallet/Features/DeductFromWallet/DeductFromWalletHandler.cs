
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Exceptions;
using Credit_Wallet.Repositories;
using Credit_Wallet.Services;
using Microsoft.EntityFrameworkCore;




namespace Credit_Wallet.Features.DeductFromWallet
{
    public class DeductFromWalletHandler
    {
        private readonly ILogger<DeductFromWalletHandler> _logger;
        private readonly DeductFromWalletValidator _validator;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HmacService _hmacService;
        private readonly WalletIntegrityService _walletIntegrityService;


        public DeductFromWalletHandler(
            ILogger<DeductFromWalletHandler> logger,
            DeductFromWalletValidator validator,
            IServiceScopeFactory scopFactory,
            HmacService hmacService,
            WalletIntegrityService walletIntegrityService)
          
        {
            _logger = logger;
            _validator = validator;
            _scopeFactory = scopFactory;
            _hmacService = hmacService;
            _walletIntegrityService = walletIntegrityService;
        }
        private async Task PerformDeductionAsync(Wallet wallet,
                                                 decimal amount,
                                                 ITransactionRepository transactionRepository,
                                                 IUnitOfWork unitOfWork)
        {
            var createDate=DateTimeHelper.NormalizeToMilliseconds(DateTime.UtcNow);
            var transactionAmount = -amount;
            var transactionData = $"{wallet.Id}|{transactionAmount:F2}|{TransactionType.Withdraw}|{createDate:O}";
            var transactionHash = _hmacService.GenerateHmacHash(transactionData);
            await transactionRepository.AddTransactionAsync(new Transaction
            {
                WalletId = wallet.Id,
                Amount = transactionAmount,
                TransactionType = TransactionType.Withdraw,
                CreatedDateTime = createDate,
                TransactionHash = transactionHash
            });

            wallet.Withdraw(amount); 
            wallet.LastUpdateDateTime =DateTimeHelper.NormalizeToMilliseconds(DateTime.UtcNow);
            wallet.RowVersion = Guid.NewGuid();
            wallet.WalletHash= _walletIntegrityService.GenerateWalletHash(wallet);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<DeductFromWalletResponse> HandleAsync(DeductFromWalletRequest request)
        {
            if (!_validator.Validate(request))
            {
                return new DeductFromWalletResponse
                {
                    Success = false,
                    Message = "Validation failed",
                };
            }
            const int maxAttempt = 2;
            for (var attempt = 1; attempt <= maxAttempt; attempt++)
            {
                try
                {
                using var scope = _scopeFactory.CreateScope();
                var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
                var walletRepository = scope.ServiceProvider.GetRequiredService<IWalletRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var wallet = await walletRepository.GetWalletByUserIdAsync(request.UserId);

                if (wallet == null)
                {
                    return new DeductFromWalletResponse
                    {
                        Success = false,
                        Message = "Wallet not found",
                    };
                }
                if(!_walletIntegrityService.VerifyWallet(wallet))
                    {
                        _logger.LogError($"Wallet integrity verification failed for userId {wallet.UserId},walletId {wallet.Id}", request.UserId,wallet.Id);
                        return new DeductFromWalletResponse
                        {
                            Success = false,
                            Message = "Unable to process the request!",
                        };
                    }
                    if (wallet.Balance < request.Amount)
                {
                    return new DeductFromWalletResponse
                    { 
                        Success = false,
                        Message = "Insufficient funds",
                        NewBalance = wallet.Balance
                    };
                }
                    await PerformDeductionAsync(wallet,
                                                request.Amount,
                                                transactionRepository,
                                                unitOfWork);

                    return new DeductFromWalletResponse
                    {
                        Success = true,
                        Message = "Amount deducted successfully",
                        NewBalance = wallet.Balance
                    };

                }
                catch (WalletConcurrencyException ex)
                {
                    _logger.LogWarning(ex, "The Wallet Is Modified By Another Request,{UserId}", request.UserId);
                    if (attempt == maxAttempt)
                    {
                        _logger.LogError("Failed to deduct amount for UserId: {UserId} after 2 attempts due to concurrency issues.", request.UserId);
                        return new DeductFromWalletResponse
                        {
                            Success = false,
                            Message = "Failed to deduct amount . Please try again.",
                        };
                        // await _walletRepository.ReloadWalletAsync(wallet);
                    }
                }
                catch(DatabaseException ex)
                {
                        _logger.LogError(ex, "Database update error while deducting amount for UserId: {UserId}", request.UserId);
                        return new DeductFromWalletResponse
                        {
                            Success = false,
                            Message = "Failed to deduct amount . Please try again.",
                        };
                }
                catch(InvalidOperationException ex)
                {
                    _logger.LogWarning(ex, "Insufficient funds for UserId: {UserId}", request.UserId);
                    return new DeductFromWalletResponse
                    {
                        Success = false,
                        Message = "Insufficient funds",
                       
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while deducting amount for UserId: {UserId}", request.UserId);
                    return new DeductFromWalletResponse
                    {
                        Success = false,
                        Message = "An error occurred while processing your request.",
                    };
                }
            }
            return new DeductFromWalletResponse
            {
                Success = false,
                Message = "Unable to process request.",
            };
        
        }
    }   
}