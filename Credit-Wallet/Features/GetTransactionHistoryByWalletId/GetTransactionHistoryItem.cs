using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Features.GetTransactionHistory
{
    public class GetTransactionHistoryItem
    {
            public int WalletId { get; set; }
            public Guid UserId { get; set; } 
        public int TransactionId { get; set; }
            public TransactionType TransactionType { get; set; }
            public decimal Amount { get; set; }
            public DateTime CreatedDateTime { get; set; }

    }
}
