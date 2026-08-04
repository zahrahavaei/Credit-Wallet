using Credit_Wallet.Data;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Wallet.Features.AddCreditToWallet
{
   

    public class AddCreditToWalletEndpoint
    {
         public static void MapAddCreditToWalletEndpoint(WebApplication app)
         {
             app.MapPost("/api/wallet/add-credit", async (AddCreditToWalletRequest request,
                                                          AddCreditToWalletHandler handler) =>
             {
                 var response = await handler.HandleAsync(request);
                 if(!response.Success)
                 {
                     return Results.BadRequest(response);
                 }
                 return Results.Ok(response);
             });
         }
        
    }
}
