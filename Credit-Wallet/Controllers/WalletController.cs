using Credit_Wallet.Features.AddCreditToWallet;
using Credit_Wallet.Features.DeductFromWallet;
using Credit_Wallet.Features.GetuserWallet;
using Credit_Wallet.Features.MakeWallet;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Wallet.Controllers;

[ApiController]
[Route("api/wallet")]
public class WalletController : ControllerBase
{
    private readonly IMakeWalletService _makeWalletService;
    private readonly AddCreditToWalletHandler _addCreditToWalletHandler;
    private readonly DeductFromWalletHandler _deductFromWalletHandler;
    private readonly GetUserWalletHandler _getUserWalletHandler;

    public WalletController(IMakeWalletService makeWalletService,
                            AddCreditToWalletHandler addCreditToWalletHandler,
                            DeductFromWalletHandler deductFromWalletHandler,
                            GetUserWalletHandler getUserWalletHandler)
    {
        _makeWalletService = makeWalletService;
        _addCreditToWalletHandler = addCreditToWalletHandler;
        _deductFromWalletHandler = deductFromWalletHandler;
        _getUserWalletHandler = getUserWalletHandler;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateWallet()
    {
        var resultId = await _makeWalletService.HandleAsync();
        
        return Ok(new {message = "Wallet created successfully",
                       walletId=resultId});
    }
    [HttpPost("add-credit")]
    public async Task<IActionResult> AddCreditAsync([FromBody] AddCreditToWalletRequest request)
    {
        var response = await _addCreditToWalletHandler.HandleAsync(request);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        else
        {
            return Ok(response);
        }
    }
    [HttpPost("deduct-credit")]
    public async Task<IActionResult> DeductCreditAsync([FromBody] DeductFromWalletRequest request)
    {
        var response = await _deductFromWalletHandler.HandleAsync(request);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        else
        {
            return Ok(response);
        }
    }
    [HttpGet("get-wallet/{userId}")]
    public async Task<IActionResult> GetWalletAsync(string userId)
    {
        var response = await _getUserWalletHandler.HandleAsync(userId);
        if (response == null)
        {
            return NotFound();
        }
        return Ok(response);
    }
}