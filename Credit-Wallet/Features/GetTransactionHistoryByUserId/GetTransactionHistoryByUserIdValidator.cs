namespace Credit_Wallet.Features.GetTransactionHistoryByUserId
{
    public class GetTransactionHistoryByUserIdValidator
    {
        private readonly ILogger<GetTransactionHistoryByUserIdValidator> _logger;
        public GetTransactionHistoryByUserIdValidator(ILogger<GetTransactionHistoryByUserIdValidator> logger)
        {
            _logger = logger;
        }
        public bool Validate(Guid userId, GetTransactionHistoryByUserIdRequest request, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (userId==Guid.Empty)
            {
                errorMessage = "User ID is required.";
                return false;
            }
            if (request.PageNumber <= 0 || request.PageNumber>100)
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
                errorMessage = "From date cannot be later than To date.";
                return false;
            }
            return true;
        }
    }
}
