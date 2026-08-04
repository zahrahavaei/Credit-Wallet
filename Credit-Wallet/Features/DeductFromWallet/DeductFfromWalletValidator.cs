namespace Credit_Wallet.Features.DeductFromWallet
{
    public class DeductFromWalletValidator
    {
        public bool Validate(DeductFromWalletRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                return false;
            }
            if (request.Amount <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
