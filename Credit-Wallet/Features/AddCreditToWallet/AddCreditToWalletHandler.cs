using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Credit_Wallet.Features.AddCreditToWallet
{
    public class AddCreditToWalletHandler
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly AddCreditToWalletValidator _validator;
        private readonly ILogger<AddCreditToWalletHandler> _logger;
        public AddCreditToWalletHandler(ApplicationDbContext context,
                                        AddCreditToWalletValidator validator,
                                        ILogger<AddCreditToWalletHandler> logger)
        {
            _dbcontext = context;
            _validator = validator;
            _logger = logger;
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
            var wallet = await _dbcontext.Wallets
                                              .FirstOrDefaultAsync(w => w.UserId == request.UserId);
            if (wallet == null)
            {
               return new AddCredittoWalletResponse
                {
                    Success = false,
                    Message = $"Wallet not found for this {request.UserId}",
                    
                };
            }
            var maxAttempt = 3;
            for(var attempt = 0; attempt < maxAttempt;attempt++)
            {
                try
                {
                    using var transaction = await _dbcontext.Database.BeginTransactionAsync();
                   _dbcontext.Transactions.RemoveRange(
                          _dbcontext.Transactions.Local.Where(t=>t.WalletId == wallet.Id &&
                                                               t.TransactionType== TransactionType.Deposit));
                    var walletTransaction = new Transaction
                    {
                        WalletId = wallet.Id,
                        Amount = request.Amount,
                        TransactionType = TransactionType.Deposit,
                        CreatedDateTime = DateTime.Now//can I remove as
                                                      //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] is used in Transaction entity
                    };
                    await _dbcontext.Transactions.AddAsync(walletTransaction);
                    wallet.Balance += request.Amount;
                    await _dbcontext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new AddCredittoWalletResponse
                    {
                        Success = true,
                        Message = "Credit added successfully",
                        NewBalance = wallet.Balance
                    };
                }
                catch(DbUpdateConcurrencyException exception)
                {
                    _logger.LogWarning("Concurrency conflict detected while adding credit to wallet for user {UserId}. Attempt {Attempt} of {MaxAttempt}. Exception: {ExceptionMessage}", request.UserId, attempt + 1, maxAttempt, exception.Message);
                    if (attempt == maxAttempt - 1)
                    {
                        return new AddCredittoWalletResponse
                        {
                            Success = false,
                            Message = "Failed to add credit to the wallet due to concurrent updates. Please try again later.",
                            
                        };
                    }
                   await _dbcontext.Entry(wallet).ReloadAsync();
                  
                }
              
            }
                return new AddCredittoWalletResponse
                {
                    Success = false,
                    Message = "Failed to add credit to the wallet after multiple attempts. Please try again later.",
                    
                };
        }
    }
}
