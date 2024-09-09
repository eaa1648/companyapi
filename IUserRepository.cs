public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<IEnumerable<User>> GetAllAsync(); // Kullanıcıları listelemek için
    Task AddAsync(User user);
    Task DeleteAsync(User user);
}
