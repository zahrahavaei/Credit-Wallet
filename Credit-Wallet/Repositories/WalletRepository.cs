using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Repositories
{
    public class WalletRepository:IWalletRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public WalletRepository(ApplicationDbContext dbContext) 
        {
            _dbcontext = dbContext;
        }
        public async Task<Wallet?> GetWalletByUserIdAsync(string userId)
        {
            return await _dbcontext.Wallets.FirstOrDefaultAsync(w=>w.UserId == userId);
        }
    }
}
