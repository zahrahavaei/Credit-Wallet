namespace Credit_Wallet.Features.GetTransactionHistoryByUserId
{
    public class GetTransactionHistoryByUserIdResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<GetTransactionHistoryByUserIdItem> Transactions { get; set; }
    }
}
