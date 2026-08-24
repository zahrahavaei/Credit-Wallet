using Credit_Wallet.Enum;
using Credit_Wallet.Features.UserLogin;
using Credit_Wallet.Features.UserRegistration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Claims;
using System.Text;
using Credit_Wallet.Enum;


namespace Credit_Wallet.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRegistrationHandler _userRegistrationHandler;
        private readonly UserLoginHandler _userLoginHandler;
        private  readonly IConfiguration _configuration;
     
        public AuthController(UserRegistrationHandler userRegistrationHandler,
                              UserLoginHandler userLoginHandler,
                              IConfiguration configuration)
        {
            _userRegistrationHandler = userRegistrationHandler;
            _userLoginHandler = userLoginHandler;
            _configuration = configuration;
           
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationRequest request)
        {
            var response = await _userRegistrationHandler.RegisterUserAsync(request);
            switch (response.Status)
            { 
                case  ResponseStatus.Success:
                    return Ok(response);
                case ResponseStatus.Error:
                    return BadRequest(response);
                default:
                    return BadRequest(response);
            }
        }//..............................................................................
        [HttpPost("login")]
        public async Task<ActionResult<UserloginResponse>> LogInUser([FromBody]UserLoginRequest request)
        {
            var response = await _userLoginHandler.UserLoginHandleAsync(request);
            if (response.Status == ResponseStatus.Success)
            {
                var token = GenerateToken(response.FirstName,
                                         response.LastName,
                                         response.UserName,
                                         response.UserRole,
                                         response.UserId);
                response.Token = token;
            }
            switch (response.Status)
            {
                case ResponseStatus.Success:
                    return Ok(response);
                case ResponseStatus.NotFound:
                    return NotFound(response);
                case ResponseStatus.IntegrityFailed:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                case ResponseStatus.PasswordVerificationFailed:
                    return BadRequest(response);
                default: return BadRequest(response);

            }
        }
        //..............................................................................
        private string GenerateToken(string firstName,
                                    string lastname,
                                    string userName,
                                    UserRole userRole,
                                    Guid userId)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }
            var keyByte=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds=new SigningCredentials(keyByte,SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim (JwtRegisteredClaimNames.Sub,userName),
                new Claim(ClaimTypes.Name,$"{firstName} {lastname}"),
                new Claim(ClaimTypes.Role,userRole.ToString()),
                new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                 issuer: _configuration["Jwt:Issuer"],
                 audience: _configuration["Jwt:Audience"],
                 claims: claims,
                 expires: DateTime.UtcNow.AddMinutes(60),
                 signingCredentials: creds
                 );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //.....................................................................................
    }
}
