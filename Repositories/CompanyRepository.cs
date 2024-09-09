using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetByUsernameAsync(string username)
    {
        return await _context.Companies
                             .SingleOrDefaultAsync(c => c.Username == username);
    }

    public async Task<Company?> GetByIdAsync(int id)
    {
        return await _context.Companies
                             .SingleOrDefaultAsync(c => c.Id == id);
    }
}
