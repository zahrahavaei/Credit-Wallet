using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Features.GetTransactionHistory
{
    public class GetTransactionHistoryItem
    {
            public int WalletId { get; set; }
            public string UserId { get; set; } = string.Empty;
        public int TransactionId { get; set; }
            public TransactionType TransactionType { get; set; }
            public decimal Amount { get; set; }
            public DateTime CreatedDateTime { get; set; }

    }
}
