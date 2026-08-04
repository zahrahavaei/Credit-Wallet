namespace Credit_Wallet.Features.DeductFromWallet
{
    public class DeductFromWalletEndpoint
    {
        public static void MapDeductFromWalletEndpoint(WebApplication app)
        {
            app.MapPost("api/wallet/deduct-credit",async(DeductFromWalletRequest request, DeductFromWalletHandler handler) =>
            {
                var result = await handler.HandleAsync(request);
               return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}
