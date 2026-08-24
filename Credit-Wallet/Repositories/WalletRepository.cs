using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Repositories
{

    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public WalletRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Wallet?> GetWalletByUserIdAsync(Guid userId)
        {
            return await _dbContext.Wallets
                                         .FirstOrDefaultAsync(w => w.UserId== userId);
           
        }
        public async Task ReloadWalletAsync(Wallet wallet)
        {
            await _dbContext.Entry(wallet).ReloadAsync();
        }
    }
}

