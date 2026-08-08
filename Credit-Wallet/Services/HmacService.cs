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
        public bool VerifyHmacHash(string data ,string storedHash)
        {
            var computedHash = GenerateHmacHash(data);
            // return computedHash == storedHash; to prevent timing attacks
            return CryptographicOperations.FixedTimeEquals( 
                   Convert.FromBase64String(computedHash),
                   Convert.FromBase64String(storedHash));
        }
    }
}
