using Credit_Wallet.Data.Entities;
using Credit_Wallet.Features.UserRegistration;
using static Credit_Wallet.Repositories.UserRepository;

namespace Credit_Wallet.Repositories
{
    public interface IUserRepository
    {
         Task<int> AddUserAsync(User user );
        Task<User?> GetUserByUserNameAsync(string UserName);
           
    }
}
