using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Exceptions
{
    public class WalletConcurrencyException : Exception
    {
        public WalletConcurrencyException(DbUpdateConcurrencyException ex)
            : base("The Wallet Is Modified By Another Request", ex)
        {
        }

        public WalletConcurrencyException(string? message)
            : base(message)
        {
        }

        public WalletConcurrencyException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}