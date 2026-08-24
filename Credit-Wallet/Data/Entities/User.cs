using Credit_Wallet.Enum;

namespace Credit_Wallet.Data.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public UserRole UserRole { get; set; } = UserRole.Customer;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastLogInDateTime { get; set; }

        public string UserHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;


        public string PhoneNumber {  get; set; } = string.Empty;

        public ICollection<Wallet> Wallets { get; set; }=new List<Wallet>();
    }
}
