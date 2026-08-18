using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Features.GetTransactionHistoryByUserId
{
    public class GetTransactionHistoryByUserIdItem
    {
        public string UserId { get; set; } = string.Empty;
        public int WalletId { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime CreatedDateTime { get; set; }
       

    }
}
