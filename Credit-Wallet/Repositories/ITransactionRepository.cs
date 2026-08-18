
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Features.GetTransactionHistoryByUserId;
using Credit_Wallet.Features.GetTransactionHistoryByWalletId;

namespace Credit_Wallet.Repositories
{
    public interface ITransactionRepository
    {
         Task AddTransactionAsync(Transaction transaction);
        Task <Transaction?>GetTransactionByIdAsync(int id);

        Task<IEnumerable<Transaction>> GetTransactionHistoryByWalletIdAsync(int walletID,
                                                                       GetTransactionHistoryByWalletIdRequest request);
        Task<IEnumerable<Transaction>>GetTransactionHistoryByUserIdAsync(string userId,
                                                                        GetTransactionHistoryByUserIdRequest request);
    }
}
