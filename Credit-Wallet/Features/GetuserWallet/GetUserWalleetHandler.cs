using Credit_Wallet.Data;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Features.GetuserWallet
{
    public class GetUserWalletHandler
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GetUserWalletHandler> _logger;
        public GetUserWalletHandler(ApplicationDbContext dbContext, 
                                     ILogger<GetUserWalletHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<GetUserWalletResponse?> HandleAsync(string userId)
        {
            var wallet = await _dbContext.Wallets.AsNoTracking()
                                                 .FirstOrDefaultAsync
                                                  (w => w.UserId == userId);
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
