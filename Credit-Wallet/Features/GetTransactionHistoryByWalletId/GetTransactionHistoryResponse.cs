using Credit_Wallet.Data.Entities;
using Credit_Wallet.Enum;

namespace Credit_Wallet.Features.GetTransactionHistory
{
    public class GetTransactionHistoryResponse
    {
        public List<GetTransactionHistoryItem> Transactions { get; set; } = new();
        public ResponseStatus Status { get; set; }
        public string Message { get; set; } = string.Empty;

        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }



    }
}
