namespace Credit_Wallet.Features.AddCreditToWallet
{
    public class AddCreditToWalletRequest
    {
        public decimal Amount { get; set; }
        public Guid UserId { get; set; }
    }
}
