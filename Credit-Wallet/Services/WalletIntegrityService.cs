using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Services
{
    public class WalletIntegrityService
    {
        private readonly HmacService _hmacService;
        private readonly ILogger<WalletIntegrityService> _logger;
        public WalletIntegrityService(HmacService hmacService,
                                      ILogger<WalletIntegrityService> logger)
        {
            _hmacService = hmacService;
            _logger = logger;
        }
        public bool VerifyWallet(Wallet wallet)
        {
            var data = $"{wallet.Id}|{wallet.UserId}|{wallet.Balance:F2}|{wallet.LastUpdateDateTime:o}";

            var calculatedHash = _hmacService.GenerateHmacHash(data);

            _logger.LogWarning(
                "VERIFY Wallet - Data: [{Data}], CalculatedHash: [{CalculatedHash}], StoredHash: [{StoredHash}]",
                data,
                calculatedHash,
                wallet.WalletHash);

            return _hmacService.VerifyHmacHash(data, wallet.WalletHash);
        }
        public string GenerateWalletHash(Wallet wallet)
        {
          
            var data = $"{wallet.Id}|{wallet.UserId}|{wallet.Balance:F2}|{wallet.LastUpdateDateTime:o}";
            var hash = _hmacService.GenerateHmacHash(data);
            _logger.LogWarning("GENERATING WalletHash - Data: [{Data}], Hash: [{Hash}]",
            data, hash);
            return hash;
        }
       
    }
}
