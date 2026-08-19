namespace Credit_Wallet.Features.GetTransactionHistoryByWalletId
{
    public class GetTransactionHistoryByWalletIdRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
