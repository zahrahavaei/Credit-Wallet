using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Repositories
{
    public class TransactionRepository: ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);

        }
    }
}
