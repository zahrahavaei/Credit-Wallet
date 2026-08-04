namespace Credit_Wallet.Features.DeductFromWallet
{
    public class DeductFromWalletResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal NewBalance { get; set; }
    }
}
