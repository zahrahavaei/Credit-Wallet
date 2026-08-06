using System.Security.Cryptography;
using System.Text;

namespace Credit_Wallet.Services
{
    public class HmacService
    {
        private readonly string _secretKey;
        public HmacService(IConfiguration config)
        {
            _secretKey = config["Hmac:Secretkey"] ??
                throw new InvalidOperationException("HMAC secret key is missing");

        }
        public string GenerateHmacHash(string data)
        {
            using var hmac=new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            var hashBytes=hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
