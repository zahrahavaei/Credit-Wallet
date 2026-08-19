namespace Credit_Wallet.Features.GetTransactionHistoryByWalletId
{
    public class GetTransactionHistoryByWalletIdValidator
    {
        public bool Validate(int walletId, GetTransactionHistoryByWalletIdRequest request, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (walletId <= 0)
            {
                errorMessage = "Invalid wallet ID.";
                return false;
            }
            if (request.PageNumber <= 0)
            {
                errorMessage = "Page number must be greater than zero.";
                return false;
            }
            if (request.PageSize <= 0)
            {
                errorMessage = "Page size must be greater than zero.";
                return false;
            }
            if (request.FromDate.HasValue && request.ToDate.HasValue && request.FromDate > request.ToDate)
            {
                errorMessage = "From date cannot be later than to date.";
                return false;
            }
            return true;
        }
    }
}
