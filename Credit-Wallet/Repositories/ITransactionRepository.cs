
using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Repositories
{
    public interface ITransactionRepository
    {
         Task AddTransactionAsync(Transaction transaction);
        Task <Transaction?>GetTransactionByIdAsync(int id);

        Task<IEnumerable<Transaction>> GetTransactionHistoryByWalletIdAsync(int walletID);
    }
}
