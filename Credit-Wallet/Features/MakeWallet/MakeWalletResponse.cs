using Credit_Wallet.Enum;

namespace Credit_Wallet.Features.MakeWallet
{
    public class MakeWalletResponse
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public int WalletId { get; set; }
    }
}
