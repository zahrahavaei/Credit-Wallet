using Credit_Wallet.Enum;

namespace Credit_Wallet.Features.UserLogin
{
    public class UserloginResponse
    {
        public ResponseStatus Status { get; set; }
        public string Message {  get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public UserRole UserRole { get; set; }
        public string Token { get; set; }
    }
}
