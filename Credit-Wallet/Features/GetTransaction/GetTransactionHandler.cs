using Credit_Wallet.Repositories;
using Credit_Wallet.Services;

namespace Credit_Wallet.Features.GetTransaction
{
    public class GetTransactionHandler
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly TransactionIntegrityService _transactionIntegrityService;
        private readonly ILogger<GetTransactionHandler> _logger;
        public GetTransactionHandler(ITransactionRepository transactionRepository,
                                     TransactionIntegrityService transactionIntegrityService,
                                     ILogger<GetTransactionHandler> logger)
        {
            _transactionRepository = transactionRepository;
            _transactionIntegrityService = transactionIntegrityService;
            _logger = logger;
        }
        public async Task<GetTransactionResponse> HandleAsync(int transactionId)
        {
            var fetchTransaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
            if (fetchTransaction == null)
            {
                return new GetTransactionResponse
                {
                    Success = false,
                    Message = "Transaction not found"
                };
            }
            var isValid = _transactionIntegrityService.VerifyTransaction(fetchTransaction);
            if (!isValid) {
                _logger.LogWarning("Transaction integrity check failed for transaction ID: {TransactionId}", transactionId);
                return new GetTransactionResponse
                {
                    Success = false,
                    Message = "Transaction integrity check failed."
                };
            }
            return new GetTransactionResponse
            {
                Success = true,
                Message = "Transaction verified successfully.",
                TransactionId = fetchTransaction.Id,
                WalletId = fetchTransaction.WalletId,
                Amount = fetchTransaction.Amount,
                TransactionType = fetchTransaction.TransactionType,
                CreatedDateTime = fetchTransaction.CreatedDateTime
            };
        }
    }
}
