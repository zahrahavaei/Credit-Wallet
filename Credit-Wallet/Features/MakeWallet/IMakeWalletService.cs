namespace Credit_Wallet.Features.MakeWallet;

public interface IMakeWalletService
{
    Task<MakeWalletResponse> HandleAsync(Guid userId);
}