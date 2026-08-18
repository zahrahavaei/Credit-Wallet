namespace Credit_Wallet.Features.GetTransactionHistoryByWalletId
{
    public class GetTransactionHistoryByWalletIdRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
