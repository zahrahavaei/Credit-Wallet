namespace Credit_Wallet.Exceptions
{
    public class WalletConcurrencyException:Exception
    {
        public WalletConcurrencyException(string message) : base(message)
        {
        }
        public WalletConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
