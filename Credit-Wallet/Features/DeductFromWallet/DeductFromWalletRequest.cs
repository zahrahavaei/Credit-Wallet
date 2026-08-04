namespace Credit_Wallet.Features.DeductFromWallet
{
    public class DeductFromWalletRequest
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
