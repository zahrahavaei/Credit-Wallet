using Credit_Wallet.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Wallet.Controllers
{
    
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

       /* [HttpGet("{transactionid}")]
        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            await _transactionRepository.GetTransactionByIdAsync(id);
        }*/
    }
}
