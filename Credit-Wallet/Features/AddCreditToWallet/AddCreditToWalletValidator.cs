

namespace Credit_Wallet.Features.AddCreditToWallet
{
    public class AddCreditToWalletValidator
    {
       public bool Validate(AddCreditToWalletRequest request)
        {
            if(request==null)
            {
                return false;
            }
            if (request.Amount<=0)
            {
                return false;
            }
            if (request.UserId == Guid.Empty)
            {
                return false;
            }
            return true;
        }
    }
}
