using Microsoft.EntityFrameworkCore; // Gerekli namespace

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        // IEnumerable<User> dönüş türü sağlamak için ToListAsync() kullanarak dönüş yapıyoruz
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
