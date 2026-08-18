using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Features.GetTransactionHistoryByUserId;
using Credit_Wallet.Features.GetTransactionHistoryByWalletId;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Repositories
{
    public class TransactionRepository : ITransactionRepository
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
        //..............................................................
        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);
        }
        //..........................................................
        public async Task<IEnumerable<Transaction>> GetTransactionHistoryByWalletIdAsync(int walletID,
                                                                       GetTransactionHistoryByWalletIdRequest request)
        {
            var query =  _dbContext.Transactions.Where(t => t.WalletId == walletID);
            if(request.FromDate.HasValue)
            {
                var FromDate=DateTime.SpecifyKind(request.FromDate.Value.Date,
                                                 DateTimeKind.Utc);
                query = query.Where(t => t.CreatedDateTime >= FromDate);
            }
            if (request.ToDate.HasValue)
            {
                var ToDate = DateTime.SpecifyKind(request.ToDate.Value.Date.AddDays(1),
                                                   DateTimeKind.Utc);
                query = query.Where(t => t.CreatedDateTime < ToDate);
            }
            var transactions = await query.ToListAsync();
            return transactions;
        }
        //................................................................
        public async Task<IEnumerable<Transaction>> GetTransactionHistoryByUserIdAsync(string userId,
                                                                         GetTransactionHistoryByUserIdRequest request)
        {
            var query =  _dbContext.Transactions.Include(t => t.Wallet)
                                                          .Where(t => t.Wallet.UserId == userId);
              if(request.FromDate.HasValue)
            {
                var FromDate = DateTime.SpecifyKind(request.FromDate.Value.Date,
                                                  DateTimeKind.Utc);
                query=query.Where(t=>t.CreatedDateTime >= FromDate);
            }
              if(request.ToDate.HasValue)
            {
                var ToDate = DateTime.SpecifyKind( request.ToDate.Value.Date.AddDays(1),
                                                   DateTimeKind.Utc);
                query = query.Where(t => t.CreatedDateTime <ToDate);
            }
              var transactions = await query.ToListAsync();
            return transactions;
        }
    }
}
