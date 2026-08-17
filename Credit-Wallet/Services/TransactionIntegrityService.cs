

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
            var data = $"{transaction.WalletId}|{transaction.Amount:F2}|{transaction.TransactionType}|{transaction.CreatedDateTime:O}";
            var calculatedHash = _hmacService.GenerateHmacHash(data);
            Console.WriteLine("data: " + data);
            Console.WriteLine("calculatedHash: " + calculatedHash);
            Console.WriteLine("transaction.hash: " + transaction.TransactionHash);
            return _hmacService.VerifyHmacHash(data, transaction.TransactionHash);
        }
       
    }
}
