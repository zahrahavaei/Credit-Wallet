using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Services;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Repositories
{

    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<WalletRepository> _logger;
        public WalletRepository(ApplicationDbContext dbContext,
                                ILogger<WalletRepository> logger)

        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Wallet> MakeWalletAsync(Guid userId)
        {
            var newWallet = new Wallet
            {
                UserId = userId,
                LastUpdateDateTime = DateTimeHelper.NormalizeToMilliseconds(DateTime.UtcNow)
            };
            try
            {
                _dbContext.Wallets.Add(newWallet);
                await _dbContext.SaveChangesAsync();
                return newWallet;
            }
            catch (Exception ex)
            {
              _logger.LogError(ex, "Error occurred while creating a new wallet for user {UserId}", userId);
                throw;
            }
            
        }
       // ...................................................................
        public async Task<Wallet?> GetWalletByUserIdAsync(Guid userId)
        {
            return await _dbContext.Wallets
                                         .FirstOrDefaultAsync(w => w.UserId== userId);
           
        }
      //  ......................................................
        public async Task ReloadWalletAsync(Wallet wallet)
        {
            await _dbContext.Entry(wallet).ReloadAsync();
        }
        //...............................
        public async Task SaveWalletAsync()
        {
            
            await _dbContext.SaveChangesAsync();
        }
    }
}

