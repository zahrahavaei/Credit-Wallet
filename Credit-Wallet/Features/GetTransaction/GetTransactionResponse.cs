using Credit_Wallet.Data.Entities;
using Credit_Wallet.Enum;
using System.Transactions;

namespace Credit_Wallet.Features.GetTransaction
{
    public class GetTransactionResponse
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public int TransactionId { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }

        public DateTime CreatedDateTime { get; set; } 
    }
}
