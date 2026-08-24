using Credit_Wallet.Data;
using Credit_Wallet.Repositories;
using Credit_Wallet.Services;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Features.GetuserWallet
{
    public class GetUserWalletHandler
    {
        private readonly IWalletRepository _walletRepository;
        private readonly WalletIntegrityService _walletIntegrityService;
        private readonly ILogger<GetUserWalletHandler> _logger;
        public GetUserWalletHandler(IWalletRepository walletRepository, 
                                   WalletIntegrityService walletIntegrityService,
                                     ILogger<GetUserWalletHandler> logger)
        {
            _walletRepository = walletRepository;
            _walletIntegrityService = walletIntegrityService;
            _logger = logger;
        }
        public async Task<GetUserWalletResponse?> HandleAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
            {
                _logger.LogWarning("Wallet not found for userId: {UserId}", userId);

                return null;
               
            }
            if (!_walletIntegrityService.VerifyWallet(wallet)) 
            {
                _logger.LogWarning("Wallet integrity check failed for userId: {UserId}", userId);
                return null;
            }
            return new GetUserWalletResponse
            {
                Balance = wallet.Balance,
                CheckedDateTime = DateTime.UtcNow
            };
        }
    }
}
