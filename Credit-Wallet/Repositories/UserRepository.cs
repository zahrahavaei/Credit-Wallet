using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Enum;
using Credit_Wallet.Features.UserLogin;
using Credit_Wallet.Features.UserRegistration;
using Credit_Wallet.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Credit_Wallet.Repositories
{
    public class UserRepository: IUserRepository
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UserRepository> _logger;
        
        public UserRepository(ApplicationDbContext dbContext ,
                              ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
           _logger = logger;
        }
        public async Task<int> AddUserAsync(User user )
        {
            try
            {
                _dbContext.Users.Add(user);
                var result = await _dbContext.SaveChangesAsync();
                return result;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex,"Error occured while adding user to database");
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError( ex, "Unexpected error while adding user {UserName}",  user.UserName);
                return 0;
            }
        }
        //.....................................................
        public async Task<User?> GetUserByUserNameAsync(string UserName)
        {
            var userfetched = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == UserName);
           
                return userfetched;
           

        }
        //..................................................
      
    }
}
