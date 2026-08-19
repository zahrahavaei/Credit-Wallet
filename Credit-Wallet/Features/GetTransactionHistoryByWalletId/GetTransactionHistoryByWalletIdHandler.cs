using Credit_Wallet.Repositories;
using Credit_Wallet.Services;
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Features.GetTransactionHistoryByWalletId;

namespace Credit_Wallet.Features.GetTransactionHistory
{
    public class GetTransactionHistoryByWalletIdHandler
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly TransactionIntegrityService _transactionIntegrityService;
        private readonly ILogger<GetTransactionHistoryByWalletIdHandler> _logger;
        private readonly GetTransactionHistooryByWalletIdValidator _validator;

        public GetTransactionHistoryByWalletIdHandler(ITransactionRepository transactionRepository,
                                            TransactionIntegrityService transactionIntegrityService,
                                            ILogger<GetTransactionHistoryByWalletIdHandler> logger,
                                            GetTransactionHistooryByWalletIdValidator validator)
        {
            _transactionRepository = transactionRepository;
            _transactionIntegrityService = transactionIntegrityService;
            _logger = logger;
            _validator = validator;
        }

        public async Task<GetTransactionHistoryResponse> HandleTransactionHistoryAsync(int walletId,
                                                             GetTransactionHistoryByWalletIdRequest request)
        {
            if (!_validator.Validate(walletId, request, out string errorMessage))
            {
                return new GetTransactionHistoryResponse
                {
                    Success = false,
                    Message = errorMessage,
                    Transactions = new List<GetTransactionHistoryItem>(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = 0,
                    TotalPages = 0
                };
            }
            var result = await _transactionRepository.GetTransactionHistoryByWalletIdAsync(walletId, request);

            bool integrityFailureOccurred = false;
            var transactionItems = new List<GetTransactionHistoryItem>();
            var totalPages = (int)Math.Ceiling((double)result.TotalCount / request.PageSize);
            foreach (var t in result.Transactions)
            {
               if(_transactionIntegrityService.VerifyTransaction(t))
                {
                    transactionItems.Add(new GetTransactionHistoryItem
                    {
                        TransactionId = t.Id,
                        WalletId = t.WalletId,
                        TransactionType = t.TransactionType,
                        Amount = t.Amount,
                        CreatedDateTime = t.CreatedDateTime
                    });
                }
                else
                {
                    _logger.LogWarning("transaction integrity failed traansactionId={transactionId},WalletId={walletId}.", t.Id ,t.WalletId  );
                    integrityFailureOccurred = true;
                }
            }
          if (!integrityFailureOccurred)
            {
                return new GetTransactionHistoryResponse
                {
                    Transactions = transactionItems,
                    Success = true,
                    Message = "Transaction history retrieved successfully.",
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = totalPages
                };
            }
            return new GetTransactionHistoryResponse
            {
                Transactions = transactionItems,
                Success = false,
                Message = "Some transactions failed integrity check. Please contact support!",
                PageNumber=request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = totalPages
            };

        }
    }
}
