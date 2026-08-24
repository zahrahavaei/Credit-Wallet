using Credit_Wallet.Enum;
using Credit_Wallet.Repositories;
using Credit_Wallet.Services;

namespace Credit_Wallet.Features.GetTransactionHistoryByUserId
{
    public class GetTransactionHistoryByUserIdHandler
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<GetTransactionHistoryByUserIdHandler> _logger;
        private  readonly TransactionIntegrityService _transactionIntegrityService;

        private readonly GetTransactionHistoryByUserIdValidator _validator;
        public GetTransactionHistoryByUserIdHandler(ITransactionRepository transactionRepository,
                                                    TransactionIntegrityService transactionIntegrityService,
                                                    ILogger<GetTransactionHistoryByUserIdHandler> logger,
                                                    GetTransactionHistoryByUserIdValidator validator)
        {
            _transactionRepository = transactionRepository;
            _transactionIntegrityService = transactionIntegrityService;
            _logger = logger;
            _validator = validator;
        }
        public async Task<GetTransactionHistoryByUserIdResponse> HandleTransactionHistoryAsync(Guid userId,
                                                                GetTransactionHistoryByUserIdRequest request)
        {
            if(!_validator.Validate(userId, request, out string errorMessage))
            {
                return new GetTransactionHistoryByUserIdResponse
                {
                    Status=ResponseStatus.InvalidRequest,
                    Message = errorMessage,
                    Transactions = new List<GetTransactionHistoryByUserIdItem>(),
                    PageSize = request.PageSize,
                    PageNumber = request.PageNumber,
                    TotalCount = 0,
                    TotalPages = 0
                };
            }
            var result = await _transactionRepository.GetTransactionHistoryByUserIdAsync(userId, request);
            if (result.TotalCount == 0)
            {
                return new GetTransactionHistoryByUserIdResponse
                {
                    Status = ResponseStatus.NotFound,
                    Message = "No Transaction Found For This UserId ${userID}",
                    Transactions = new List<GetTransactionHistoryByUserIdItem>(),
                    PageSize = request.PageSize,
                    PageNumber = request.PageNumber,
                    TotalCount = 0,
                    TotalPages = 0

                };
            }
            var transactionItems= new List<GetTransactionHistoryByUserIdItem>();
            var integrityFailureOccurred = false;
            var totalPages = (int)Math.Ceiling((double)result.TotalCount / request.PageSize);
            foreach (var t in result.Transactions)
            {
                if (_transactionIntegrityService.VerifyTransaction(t))
                {
                    transactionItems.Add(new GetTransactionHistoryByUserIdItem
                    {
                        UserId = t.Wallet.UserId,
                        WalletId = t.WalletId,
                        TransactionId = t.Id,
                        Amount = t.Amount,
                        TransactionType = t.TransactionType,
                        CreatedDateTime = t.CreatedDateTime,
                      
                    });
                }
                else
                {
                    _logger.LogWarning($"Transaction with ID {t.Id} UserID ,{t.Wallet.UserId},walletId {t.WalletId} failed integrity check and was excluded from the response.");
                    integrityFailureOccurred=true;
                }
            }
            if (!integrityFailureOccurred)
            {
                return new GetTransactionHistoryByUserIdResponse
                {
                    Status = ResponseStatus.Success,
                    Message = "Transaction history retrieved successfully.",
                    Transactions = transactionItems,
                    PageSize = request.PageSize,
                    PageNumber = request.PageNumber,
                    TotalCount = result.TotalCount,
                    TotalPages = totalPages
                };
            }
            else
            {
                return new GetTransactionHistoryByUserIdResponse
                {
                   Status= ResponseStatus.IntegrityFailed,
                    Message = "Some transactions failed integrity check.",
                    Transactions = transactionItems,
                    PageSize = request.PageSize,
                    PageNumber = request.PageNumber,
                    TotalCount = result.TotalCount,
                    TotalPages = totalPages
                };
            }
        }
           
    }
}
