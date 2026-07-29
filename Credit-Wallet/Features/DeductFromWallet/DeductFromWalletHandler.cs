using Azure.Core;
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Exceptions;
using Credit_Wallet.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;



namespace Credit_Wallet.Features.DeductFromWallet
{
    public class DeductFromWalletHandler
    {
        private readonly ILogger<DeductFromWalletHandler> _logger;
        private readonly DeductFromWalletValidator _validator;
        private readonly IServiceScopeFactory _scopeFactory;


        public DeductFromWalletHandler(
            ILogger<DeductFromWalletHandler> logger,
            DeductFromWalletValidator validator,
            IServiceScopeFactory scopFactory
          )
        {
            _logger = logger;
            _validator = validator;
            _scopeFactory = scopFactory;
        }
        private async Task PerformDeductionAsync(Wallet wallet,
                                                 decimal amount,
                                                 ITransactionRepository transactionRepository,
                                                 IUnitOfWork unitOfWork)
        {
            await transactionRepository.AddTransactionAsync(new Transaction
            {
                WalletId = wallet.Id,
                Amount = -amount,
                TransactionType = TransactionType.Withdraw,
                CreatedDateTime = DateTime.Now
            });

            wallet.Balance -= amount;
            wallet.LastUpdateDateTime = DateTime.Now;

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
                if (wallet.Balance < request.Amount)
                {
                    return new DeductFromWalletResponse
                    { 
                        Success = false,
                        Message = "Insufficient funds",
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