using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Repositories
{
    public interface IWalletRepository
    {
         Task<Wallet?> GetWalletByUserIdAsync(string userId);
        Task ReloadWalletAsync(Wallet wallet);
    }
}
