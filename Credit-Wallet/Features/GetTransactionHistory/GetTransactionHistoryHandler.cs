using Credit_Wallet.Repositories;
using Credit_Wallet.Services;
using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Features.GetTransactionHistory
{
    public class GetTransactionHistoryHandler
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly TransactionIntegrityService _transactionIntegrityService;
        private readonly ILogger<GetTransactionHistoryHandler> _logger;

        public GetTransactionHistoryHandler(ITransactionRepository transactionRepository,
                                            TransactionIntegrityService transactionIntegrityService,
                                            ILogger<GetTransactionHistoryHandler> logger)
        {
            _transactionRepository = transactionRepository;
            _transactionIntegrityService = transactionIntegrityService;
            _logger = logger;
        }

        public async Task<GetTransactionHistoryResponse> HandleTransactionHistoryAsync(int walletId)
        {
            var transactions = await _transactionRepository.GetTransactionHistoryByWalletIdAsync(walletId);
            bool integrityFailureOccurred = false;
            var transactionItems = new List<GetTransactionHistoryItem>();
            foreach (var t in transactions)
            {
               if(_transactionIntegrityService.VerifyTransaction(t))
                {
                   // validTransactions.Add(t);
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
                    Message = "Transaction history retrieved successfully."
                };
            }
            return new GetTransactionHistoryResponse
            {
                Transactions = transactionItems,
                Success = false,
                Message = "Some transactions failed integrity check. Please contact support!"
            };

        }
    }
}
