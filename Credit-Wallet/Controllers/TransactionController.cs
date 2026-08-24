using Credit_Wallet.Enum;
using Credit_Wallet.Features.GetTransaction;
using Credit_Wallet.Features.GetTransactionHistory;
using Credit_Wallet.Features.GetTransactionHistoryByUserId;
using Credit_Wallet.Features.GetTransactionHistoryByWalletId;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;

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
        public async Task<ActionResult<GetTransactionResponse>> GetTransactionByIdAsync(int transactionid)
        {
          var response= await _getTransactionHandler.HandleAsync(transactionid);
           switch(response.Status)
            {
                case ResponseStatus.Success:
                    return Ok(response);
                case ResponseStatus.IntegrityFailed:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                case ResponseStatus.NotFound:
                    return NotFound(response);
                case ResponseStatus.InvalidRequest:
                    return BadRequest(response);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        //...........................................................................................................
        [HttpGet("history/wallet/{walletId}")]
        public async Task<ActionResult<GetTransactionHistoryResponse>>GetTransactionHistoryAsync(int walletId,
                                                          [FromQuery]GetTransactionHistoryByWalletIdRequest request)
        {
           var response=   await _getTransactionHistoryByWalletIdHandler.HandleTransactionHistoryAsync(walletId, request);
            switch (response.Status)
            {
                case ResponseStatus.Success:
                    return Ok(response);
                case ResponseStatus.NotFound:
                    return NotFound(response);
                case ResponseStatus.IntegrityFailed:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                case ResponseStatus.InvalidRequest:
                    return BadRequest(response);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        //...........................................................................................................
        [HttpGet("history/user/{userId}")]
        public async Task <ActionResult<GetTransactionHistoryByUserIdResponse>> GetTransactionHistoryByUserIdAsync(Guid userId,
                                                                        [FromQuery]GetTransactionHistoryByUserIdRequest request)
        {
            var response = await _getTransactionHistoryByUserIdHandler.HandleTransactionHistoryAsync(userId, request);
           switch(response.Status)
            {
                case ResponseStatus.Success:
                    return Ok(response);
                case ResponseStatus.NotFound:
                    return NotFound(response);
                case ResponseStatus.IntegrityFailed:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                case ResponseStatus.InvalidRequest:
                    return BadRequest(response);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        //...........................................................................................................
    }
}
