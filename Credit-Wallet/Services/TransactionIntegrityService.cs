

using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Services
{
    public class TransactionIntegrityService
    {
        private readonly HmacService _hmacService;
        public TransactionIntegrityService(HmacService hmacService)
        {
            _hmacService = hmacService;
        }
        public bool VerifyTransaction(Transaction transaction)
        {
            var data = $"{transaction.WalletId}|{transaction.Amount}|{transaction.TransactionType}|{transaction.CreatedDateTime}";
            return _hmacService.VerifyHmacHash(data, transaction.TransactionHash);
        }
       
    }
}
