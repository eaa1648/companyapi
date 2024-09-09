public interface ICompanyRepository
{
    Task<Company?> GetByUsernameAsync(string username);
    Task<Company?> GetByIdAsync(int id); // Id ile getirme metodu
}
