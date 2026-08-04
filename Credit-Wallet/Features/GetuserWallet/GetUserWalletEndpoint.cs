using Credit_Wallet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Features.GetuserWallet
{
    public static class GetUserWalletEndpoint
    {
        public static void MapGetUserWalletEndpoint( IEndpointRouteBuilder app)
        {
            app.MapGet("api/wallet/{userId}", async (string userId, [FromServices] GetUserWalletHandler handler) =>
            {
               
                if(string.IsNullOrWhiteSpace(userId))
                {
                    return Results.BadRequest("Invalid UserId !");
                }
                var response =await handler.HandleAsync(userId);
                if(response == null)
                {
                    return Results.NotFound("Wallet not found for the specified UserId.");
                }

                return Results.Ok(response);
            });
        }
    }
}
