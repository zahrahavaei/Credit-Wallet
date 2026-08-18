using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Features.GetTransactionHistory
{
    public class GetTransactionHistoryResponse
    {
        public List<GetTransactionHistoryItem> Transactions { get; set; } = new();
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

       

    }
}
