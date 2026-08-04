using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Features.MakeWallet;

public class MakeWalletService : IMakeWalletService
{
    private readonly ApplicationDbContext _dbContext;

    public MakeWalletService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> HandleAsync()
    {
        var newWallet = CreateNewWallet();
        
        _dbContext.Wallets.Add(newWallet);
        await _dbContext.SaveChangesAsync();

        return newWallet.Id;
    }

    private static Wallet CreateNewWallet()
    {
        var newWallet = new Wallet
        {
            UserId = Guid.NewGuid().ToString(),
            Balance = 0,
            LastUpdateDateTime = DateTime.UtcNow
        };
        return newWallet;
    }
}