using Credit_Wallet.Features.GetTransaction;
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
        public TransactionController(GetTransactionHandler getTransactionHandler)
        {
            _getTransactionHandler = getTransactionHandler;
        }
            

        [HttpGet("{transactionid}")]
        public async Task<GetTransactionResponse> GetTransactionByIdAsync(int transactionid)
        {
          return  await _getTransactionHandler.HandleAsync(transactionid);
        }
    }
}
