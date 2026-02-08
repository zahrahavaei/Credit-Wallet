using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Credit_Wallet.Features.DeductFromWallet
{
    public class DeductFromWalletHandler
    {
        private readonly ILogger<DeductFromWalletHandler> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly DeductFfromWalletValidator _validator;

        public DeductFromWalletHandler(
            ILogger<DeductFromWalletHandler> logger,
            ApplicationDbContext dbContext,
            DeductFfromWalletValidator validator)
        {
            _logger = logger;
            _dbContext = dbContext;
            _validator = validator;
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
            var wallet = await _dbContext.Wallets
                .FirstOrDefaultAsync(w => w.UserId == request.UserId);

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
            int maxRetry = 3;

            for (int attempt = 0; attempt < maxRetry; attempt++)
            {
                try
                {
                    using var transaction = await _dbContext.Database.BeginTransactionAsync();

                    _dbContext.Transactions.RemoveRange(
                        _dbContext.Transactions.Local
                        .Where(t => t.WalletId == wallet.Id && t.TransactionType == TransactionType.Withdraw));

                    await _dbContext.Transactions.AddAsync(new Transaction
                    {
                        WalletId = wallet.Id,
                        Amount = -request.Amount,
                        TransactionType = TransactionType.Withdraw,
                        CreatedDateTime = DateTime.Now
                    });

                    wallet.Balance -= request.Amount;
                    wallet.LastUpdateDateTime = DateTime.Now;

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new DeductFromWalletResponse
                    {
                        Success = true,
                        Message = "Amount deducted successfully",
                        NewBalance = wallet.Balance
                    };
                }
                catch (DbUpdateConcurrencyException ex) 
                {
                    _logger.LogWarning(ex, "Concurrency conflict for UserId: {UserId}, attempt {Attempt}", request.UserId, attempt + 1);

                    if (attempt == maxRetry - 1)
                    {
                        return new DeductFromWalletResponse
                        {
                            Success = false,
                            Message = "Concurrency conflict occurred. Please try again.",
                        };
                    }
                    await _dbContext.Entry(wallet).ReloadAsync();
                    if (wallet.Balance < request.Amount)
                    {
                        return new DeductFromWalletResponse
                        {
                            Success = false,
                            Message = "Insufficient funds after retry",
                        };
                    }
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