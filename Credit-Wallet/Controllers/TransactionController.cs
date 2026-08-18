using Credit_Wallet.Features.GetTransaction;
using Credit_Wallet.Features.GetTransactionHistory;
using Credit_Wallet.Features.GetTransactionHistoryByUserId;
using Credit_Wallet.Features.GetTransactionHistoryByWalletId;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Wallet.Controllers
{
    
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly GetTransactionHandler _getTransactionHandler;
        private readonly GetTransactionHistoryByWalletIdHandler _getTransactionHistoryByWalletIdHandler;
        private readonly GetTransactionHistoryByUserIdHandler _getTransactionHistoryByUserIdHandler;
        public TransactionController(GetTransactionHandler getTransactionHandler,
                                     GetTransactionHistoryByWalletIdHandler getTransactionHistoryHandler,
                                     GetTransactionHistoryByUserIdHandler getTransactionHistoryByUserIdHandler)
        {
            _getTransactionHandler = getTransactionHandler;
            _getTransactionHistoryByWalletIdHandler = getTransactionHistoryHandler;
            _getTransactionHistoryByUserIdHandler = getTransactionHistoryByUserIdHandler;
        }
            

        [HttpGet("{transactionid}")]
        public async Task<GetTransactionResponse> GetTransactionByIdAsync(int transactionid)
        {
          return  await _getTransactionHandler.HandleAsync(transactionid);
        }
        //...........................................................................................................
        [HttpGet("history/wallet/{walletId}")]
        public async Task<GetTransactionHistoryResponse> GetTransactionHistoryAsync(int walletid,
                                                          [FromQuery]GetTransactionHistoryByWalletIdRequest request)
        {
           var response=   await _getTransactionHistoryByWalletIdHandler.HandleTransactionHistoryAsync(walletid,request);
            return response;

        }
        //...........................................................................................................
        [HttpGet("history/user/{userId}")]
        public async Task <GetTransactionHistoryByUserIdResponse> GetTransactionHistoryByUserIdAsync(string userId,
                                                                        [FromQuery]GetTransactionHistoryByUserIdRequest request)
        {
            var response = await _getTransactionHistoryByUserIdHandler.HandleTransactionHistoryAsync(userId,
                                                                                                    request);
            return response;
        }
        //...........................................................................................................
    }
}
