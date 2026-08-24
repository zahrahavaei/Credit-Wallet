using Credit_Wallet.Data;
using Credit_Wallet.Data.Entities;
using Credit_Wallet.Enum;
using Credit_Wallet.Repositories;
using Credit_Wallet.Services;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Eventing.Reader;
using System.Net.NetworkInformation;

namespace Credit_Wallet.Features.UserRegistration
{
    public class UserRegistrationHandler
    {
        private readonly HmacService _hmacService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;
        
        public UserRegistrationHandler(HmacService hmacSeervice,
                                        IPasswordHasher<User> passwordHasher,
                                        IUserRepository userRepository)
        {
            _hmacService=hmacSeervice;
            _passwordHasher=passwordHasher;
            _userRepository=userRepository;
        }
        public async Task <UserRegisterationResponse> RegisterUserAsync(UserRegistrationRequest request)                                 
        {
            var userFetched = await _userRepository.GetUserByUserNameAsync(request.Email);
            if (userFetched != null)
            {
                return new UserRegisterationResponse
                {
                    Status = ResponseStatus.InvalidRequest,
                    Message = $"A User with userName {request.Email} exist"
                };
            }
            var user = CreateNewUser(request);

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            var data =
     $"{user.UserId}|{user.UserName}|{user.Email}|{user.FirstName}|{user.LastName}|{user.PhoneNumber}|{user.PasswordHash}";
            user.UserHash=_hmacService.GenerateHmacHash(data);

            var result = await _userRepository.AddUserAsync(user);
            if (result > 0) {
                return new UserRegisterationResponse
                {
                    Status = ResponseStatus.Success,
                    Message = $" User {user.FirstName} {user.LastName} " +
                    $"Registered Successfuly with userName {user.UserName}!",
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Username = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                };
                }
            else
            {
                return new UserRegisterationResponse
                {
                    Status = ResponseStatus.Error,
                    Message = $" User  Registeration Failed !",
                };
            }
           
        }
        private User CreateNewUser(UserRegistrationRequest request )
        {
            var user = new User
            {
                
                UserId = Guid.NewGuid(),
                CreatedDateTime = DateTime.UtcNow,
                LastLogInDateTime = DateTime.UtcNow,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName= request.Email,
                PhoneNumber = request.PhoneNumber,
                

            };
            return user;
        }
    }
}
