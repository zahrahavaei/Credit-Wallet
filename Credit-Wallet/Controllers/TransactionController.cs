using Credit_Wallet.Features.GetTransaction;
using Credit_Wallet.Features.GetTransactionHistory;
using Credit_Wallet.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Wallet.Controllers
{
    
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly GetTransactionHandler _getTransactionHandler;
        private readonly GetTransactionHistoryHandler _GetTransactionHistoryHandler;
        public TransactionController(GetTransactionHandler getTransactionHandler,
                                     GetTransactionHistoryHandler getTransactionHistoryHandler)
        {
            _getTransactionHandler = getTransactionHandler;
            _GetTransactionHistoryHandler = getTransactionHistoryHandler;
        }
            

        [HttpGet("{transactionid}")]
        public async Task<GetTransactionResponse> GetTransactionByIdAsync(int transactionid)
        {
          return  await _getTransactionHandler.HandleAsync(transactionid);
        }

        [HttpGet("history/{walletid}")]
        public async Task<GetTransactionHistoryResponse> GetTransactionHistoryAsync(int walletid)
        {
           var response=   await _GetTransactionHistoryHandler.HandleTransactionHistoryAsync(walletid);
            return response;
        }
    }
}
