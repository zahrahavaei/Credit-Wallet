namespace Credit_Wallet.Features.AddCreditToWallet
{
    public class AddCredittoWalletResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }=string.Empty;
        public decimal NewBalance { get; set; }
    }
}
