using Credit_Wallet.Enum;

namespace Credit_Wallet.Features.GetTransactionHistoryByUserId
{
    public class GetTransactionHistoryByUserIdResponse
    {
      public ResponseStatus Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<GetTransactionHistoryByUserIdItem> Transactions { get; set; } = new();

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

    }
}
