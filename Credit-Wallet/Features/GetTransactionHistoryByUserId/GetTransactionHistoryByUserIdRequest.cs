namespace Credit_Wallet.Features.GetTransactionHistoryByUserId
{
    public class GetTransactionHistoryByUserIdRequest
    {
       
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
