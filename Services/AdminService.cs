using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic; // List kullanımı için

public class AdminService
{
    private readonly IAdminRepository _adminRepository; // Admin veritabanı erişim katmanı
    private readonly IUserRepository _userRepository;  // Kullanıcı veritabanı erişim katmanı
    private readonly JwtSettings _jwtSettings;

    public AdminService(IAdminRepository adminRepository, IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
    {
        _adminRepository = adminRepository;
        _userRepository = userRepository; // Kullanıcı repository'sini tanımladık
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result> RegisterAdminAsync(AdminRegistrationModel model)
    {
        var existingAdmin = await _adminRepository.GetByUsernameAsync(model.Username);
        if (existingAdmin != null)
        {
            return new Result { IsSuccess = false, Errors = new[] { "Admin already exists" } };
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var admin = new Admin
        {
            Username = model.Username,
            Email = model.Email,
            PasswordHash = hashedPassword
        };

        await _adminRepository.AddAsync(admin);
        return new Result { IsSuccess = true };
    }

    public async Task<Result> AuthenticateAsync(AdminLoginModel model)
    {
        var admin = await _adminRepository.GetByUsernameAsync(model.Username);
        if (admin == null || !BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash))
        {
            return new Result { IsSuccess = false, Errors = new[] { "Invalid username or password" } };
        }

        var token = GenerateJwtToken(admin);
        return new Result { IsSuccess = true, Data = new { Token = token } };
    }

    public async Task<Result> ListUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return new Result
        {
            IsSuccess = true,
            Data = users
        };
    }

    public async Task<Result> DeleteUserAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            return new Result { IsSuccess = false, Errors = new[] { "User not found" } };
        }

        await _userRepository.DeleteAsync(user);
        return new Result { IsSuccess = true };
    }

   public async Task<Result> CreateSubAdminAsync(CreateSubAdminModel model)
{
    var existingAdmin = await _adminRepository.GetByUsernameAsync(model.Username);
    if (existingAdmin != null)
    {
        return new Result { IsSuccess = false, Errors = new[] { "Admin already exists" } };
    }

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

    var subAdmin = new Admin
    {
        Username = model.Username,
        Email = model.Email,
        PasswordHash = hashedPassword,
        Role = model.Role // Dinamik olarak rol ismini atıyoruz
    };

    await _adminRepository.AddAsync(subAdmin);
    return new Result { IsSuccess = true, Data = "Sub Admin created successfully" };
}



    private string GenerateJwtToken(Admin admin)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key); // JWT anahtarını al
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, "Admin")
            }),
            Expires = DateTime.UtcNow.AddHours(1), // Token geçerlilik süresi
            Issuer = _jwtSettings.Issuer, // Token yayımcısı
            Audience = _jwtSettings.Audience, // Token alıcısı
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // İmzalama yöntemini belirle
        };

        var token = tokenHandler.CreateToken(tokenDescriptor); // Token oluştur
        return tokenHandler.WriteToken(token); // Token'ı döndür
    }

    
}
