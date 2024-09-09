using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;




public class CompanyService
{
    private readonly ICompanyRepository _companyRepository; // Şirket veritabanı erişim katmanı
    private readonly IUserRepository _userRepository; // Kullanıcı veritabanı erişim katmanı

    public CompanyService(ICompanyRepository companyRepository, IUserRepository userRepository)
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> CreateSubUserAsync(CreateSubUserModel model)
{
    // Şirketi kullanıcı adıyla bulma (Burada CompanyUsername yerine CompanyId kullanılabilir)
    var company = await _companyRepository.GetByIdAsync(model.CompanyId); // CompanyId kullanılabilir
    if (company == null)
    {
        return new Result { IsSuccess = false, Errors = new[] { "Company not found" } };
    }

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
        PasswordHash = hashedPassword,
        CompanyId = company.Id
    };

    await _userRepository.AddAsync(user);
    return new Result { IsSuccess = true };
}

}
