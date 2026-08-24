

namespace Credit_Wallet.Features.UserRegistration
{
    public class UserRegistrationRequest
    {
      public  string Email { get; set; }=string.Empty;
        public  string FirstName { get; set; } = string.Empty;
        public string LastName {  get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
