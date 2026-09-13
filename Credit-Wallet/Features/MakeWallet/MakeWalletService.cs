using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Enum;
using Credit_Wallet.Repositories;
using Credit_Wallet.Services;

namespace Credit_Wallet.Features.MakeWallet;

public class MakeWalletService : IMakeWalletService
{
    private readonly WalletIntegrityService _walletIntegrityService;
    private readonly WalletRepository _walletRepository;

    public MakeWalletService(  WalletIntegrityService walletIntegrityService,
                             WalletRepository walletRepository)
    {
        _walletIntegrityService = walletIntegrityService;
        _walletRepository = walletRepository;
    }

    public async Task<MakeWalletResponse> HandleAsync(Guid userId)
    {
       
        var existingWallet = await _walletRepository.GetWalletByUserIdAsync(userId);
        if (existingWallet != null)
        {
            return new MakeWalletResponse
            {
                Status = ResponseStatus.InvalidRequest,
                Message = "Wallet already exists for this user."
            };
        }
        try
        {
            var newWallet = await _walletRepository.MakeWalletAsync(userId);


            newWallet.WalletHash = _walletIntegrityService.GenerateWalletHash(newWallet);
            await _walletRepository.SaveWalletAsync();
            return new MakeWalletResponse
            {
                Status = ResponseStatus.Success,
                Message = "Wallet created successfully.",
                WalletId = newWallet.Id
            };
        }
        catch (Exception ex)
        {
            return new MakeWalletResponse
            {
                Status = ResponseStatus.Error,
                Message = $"An error occurred while creating the wallet"
            };
        }
    }

   
    
}