using Credit_Wallet.Enum;

namespace Credit_Wallet.Features.UserRegistration
{
    public class UserRegisterationResponse
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
       public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
    }
}
