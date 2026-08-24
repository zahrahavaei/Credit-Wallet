using Credit_Wallet.Data.Entities;

namespace Credit_Wallet.Services
{
    public class UserIntegrityService
    {
        private readonly HmacService _hmacService;
        public UserIntegrityService(HmacService hmacService)
        {
            _hmacService = hmacService;
        }
        public bool VerifyUser(User user)
        {
          var data=  $"{user.UserId}|{user.UserName}|{user.Email}|{user.FirstName}|{user.LastName}|{user.PhoneNumber}|{user.PasswordHash}";
         var VerifyUser=  _hmacService.VerifyHmacHash(data, user.UserHash);
          return VerifyUser;
        }
    }
}
