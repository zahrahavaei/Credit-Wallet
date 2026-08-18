using Credit_Wallet.Repositories;
using Credit_Wallet.Services;
using System.Runtime.CompilerServices;

namespace Credit_Wallet.Features.GetTransactionHistoryByUserId
{
    public class GetTransactionHistoryByUserIdHandler
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<GetTransactionHistoryByUserIdHandler> _logger;
        private  readonly TransactionIntegrityService _transactionIntegrityService;
        public GetTransactionHistoryByUserIdHandler(ITransactionRepository transactionRepository,
                                                    TransactionIntegrityService transactionIntegrityService,
                                                    ILogger<GetTransactionHistoryByUserIdHandler> logger)
        {
            _transactionRepository = transactionRepository;
            _transactionIntegrityService = transactionIntegrityService;
            _logger = logger;
        }
        public async Task<GetTransactionHistoryByUserIdResponse> HandleTransactionHistoryAsync(string userId,
                                                                GetTransactionHistoryByUserIdRequest request)
        {
            var transaction = await _transactionRepository.GetTransactionHistoryByUserIdAsync(userId, request);
            var transactionItems= new List<GetTransactionHistoryByUserIdItem>();
            var integrityFailureOccurred = false;
            foreach (var t in transaction)
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
                        CreatedDateTime = t.CreatedDateTime
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
                    Success = true,
                    Message = "Transaction history retrieved successfully.",
                    Transactions = transactionItems
                };
            }
            else
            {
                return new GetTransactionHistoryByUserIdResponse
                {
                    Success = false,
                    Message = "Some transactions failed integrity check.",
                    Transactions = transactionItems
                };
            }
        }
           
    }
}
