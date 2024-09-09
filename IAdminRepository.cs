public interface IAdminRepository
{
    Task<Admin?> GetByUsernameAsync(string username);
    Task AddAsync(Admin admin);
}
