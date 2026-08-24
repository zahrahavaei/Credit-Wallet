using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Services;

namespace Credit_Wallet.Features.MakeWallet;

public class MakeWalletService : IMakeWalletService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly WalletIntegrityService _walletIntegrityService;

    public MakeWalletService(ApplicationDbContext dbContext,
                             WalletIntegrityService walletIntegrityService)
    {
        _dbContext = dbContext;
        _walletIntegrityService = walletIntegrityService;
    }

    public async Task<int> HandleAsync(Guid userId)
    {
        var newWallet = CreateNewWallet(userId);
        
        _dbContext.Wallets.Add(newWallet);
         await _dbContext.SaveChangesAsync();
       
        newWallet.WalletHash = _walletIntegrityService.GenerateWalletHash(newWallet);
        await _dbContext.SaveChangesAsync();
        return newWallet.Id;
    }

    private  Wallet CreateNewWallet(Guid userId)
    {

        var newWallet = new Wallet
        {
            UserId = userId,
            LastUpdateDateTime = DateTimeHelper.NormalizeToMilliseconds(DateTime.UtcNow)
        };
        return newWallet;
    }
    
}