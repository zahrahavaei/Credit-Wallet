using Credit_Wallet.Repositories;

namespace Credit_Wallet.Exceptions
{
    public class WalletConcurrencyException : Exception

    {
        public WalletConcurrencyException(Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex)
                                        :base("The Wallet Is Modified By Another Request")
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
