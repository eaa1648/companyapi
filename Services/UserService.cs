using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;



public class UserService
{
    private readonly IUserRepository _userRepository; // Kullanıcı veritabanı erişim katmanı
    private readonly JwtSettings _jwtSettings;

    public UserService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result> RegisterUserAsync(UserRegistrationModel model)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(model.Username);
        if (existingUser != null)
        {
            return new Result { IsSuccess = false, Errors = new[] { "User already exists" } };
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            PasswordHash = hashedPassword
        };

        await _userRepository.AddAsync(user);
        return new Result { IsSuccess = true };
    }

    public async Task<Result> AuthenticateAsync(UserLoginModel model)
    {
        var user = await _userRepository.GetByUsernameAsync(model.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            return new Result { IsSuccess = false, Errors = new[] { "Invalid username or password" } };
        }

        var token = GenerateJwtToken(user);
        return new Result { IsSuccess = true, Data = new { Token = token } };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
