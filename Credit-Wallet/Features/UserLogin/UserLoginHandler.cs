using Credit_Wallet.Data.Entities;
using Credit_Wallet.Enum;
using Credit_Wallet.Repositories;
using Credit_Wallet.Services;
using Microsoft.AspNetCore.Identity;

namespace Credit_Wallet.Features.UserLogin
{
    public class UserLoginHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly HmacService _hmacService;
        private readonly ILogger<UserLoginHandler> _logger;

        public UserLoginHandler(IUserRepository userRepository,
                                IPasswordHasher<User> passwordHasher,
                                HmacService hmacService,
                                ILogger<UserLoginHandler> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _hmacService = hmacService;
            _logger = logger;
        }
        public async Task<UserloginResponse> UserLoginHandleAsync(UserLoginRequest request)
        {
            var userfetched = await _userRepository.GetUserByUserNameAsync(request.UserName);
            if (userfetched == null)
            {
                return new UserloginResponse
                {
                    Status = ResponseStatus.NotFound,
                    Message = $"user not found  !",
                };
            }
            var data =
$"{userfetched.UserId}|{userfetched.UserName}|{userfetched.Email}|{userfetched.FirstName}|{userfetched.LastName}|{userfetched.PhoneNumber}|{userfetched.PasswordHash}";
            var userHashVerifivation = _hmacService.VerifyHmacHash(data, userfetched.UserHash);

            if (!userHashVerifivation)
            {
                _logger.LogWarning("user integrity for {UserId} failed ", userfetched.UserId);
                return new UserloginResponse
                {
                    Status = Enum.ResponseStatus.IntegrityFailed,
                    Message = "UnEable to proceed the request !",
                    UserId = userfetched.UserId,
                    UserName = request.UserName,
                    FirstName = userfetched.FirstName,
                    LastName = userfetched.LastName,
                    UserRole = userfetched.UserRole
                };
            }

            var passwordVerification = _passwordHasher.VerifyHashedPassword(userfetched,
                                                                  userfetched.PasswordHash, request.Password);
            var passwordIsValid = passwordVerification != PasswordVerificationResult.Failed;
            if (!passwordIsValid)
            {
                _logger.LogWarning("user's password verification for {UserId} failed ", userfetched.UserId);
                return new UserloginResponse
                {
                    Status = ResponseStatus.PasswordVerificationFailed,
                    Message = "Wrong password !",
                    UserId = userfetched.UserId,
                    UserName = request.UserName,
                    FirstName = userfetched.FirstName,
                    LastName = userfetched.LastName,
                    UserRole= userfetched.UserRole
                };
            }

            if (passwordIsValid && userHashVerifivation)
            {
                return new UserloginResponse
                {
                    Status = ResponseStatus.Success,
                    Message = "user loged in successfully!",
                    UserId = userfetched.UserId,
                    UserName = request.UserName,
                    FirstName = userfetched.FirstName,
                    LastName = userfetched.LastName,
                    UserRole = userfetched.UserRole
                };
            }
           
           
            return new UserloginResponse
            {
                Status = Enum.ResponseStatus.Error,
                Message = $"Error  !",
            };
        }

    }
}
