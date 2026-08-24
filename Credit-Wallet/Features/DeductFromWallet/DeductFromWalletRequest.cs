namespace Credit_Wallet.Features.DeductFromWallet
{
    public class DeductFromWalletRequest
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
