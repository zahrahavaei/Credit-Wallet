using Credit_Wallet.Data;
using Credit_Wallet.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Features.GetuserWallet
{
    public class GetUserWalletHandler
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ILogger<GetUserWalletHandler> _logger;
        public GetUserWalletHandler(IWalletRepository walletRepository, 
                                     ILogger<GetUserWalletHandler> logger)
        {
            _walletRepository = walletRepository;
            _logger = logger;
        }
        public async Task<GetUserWalletResponse?> HandleAsync(string userId)
        {
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
            {
                _logger.LogWarning("Wallet not found for userId: {UserId}", userId);

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
