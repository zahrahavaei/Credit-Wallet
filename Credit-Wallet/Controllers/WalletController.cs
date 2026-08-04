using Credit_Wallet.Features.AddCreditToWallet;
using Credit_Wallet.Features.MakeWallet;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Wallet.Controllers;

[ApiController]
[Route("api/wallet")]
public class WalletController : ControllerBase
{
    private readonly IMakeWalletService _makeWalletService;
    private readonly AddCreditToWalletHandler _addCreditToWalletHandler;

    public WalletController(IMakeWalletService makeWalletService,
                            AddCreditToWalletHandler addCreditToWalletHandler)
    {
        _makeWalletService = makeWalletService;
        _addCreditToWalletHandler = addCreditToWalletHandler;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateWallet()
    {
        var resultId = await _makeWalletService.HandleAsync();
        
        return Ok(new {message = "Wallet created successfully"});
    }
    [HttpPost("api/wallet/add-credit")]
    public async Task<IActionResult> AddCreditAsync([FromBody] AddCreditToWalletRequest request)
    {
        var response = await _addCreditToWalletHandler.HandleAsync(request);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
}