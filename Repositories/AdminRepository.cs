using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _context;

    public AdminRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Admin?> GetByUsernameAsync(string username)
    {
        return await _context.Admins
                             .SingleOrDefaultAsync(a => a.Username == username);
    }

    public async Task AddAsync(Admin admin)
    {
        await _context.Admins.AddAsync(admin);
        await _context.SaveChangesAsync();
    }
}
