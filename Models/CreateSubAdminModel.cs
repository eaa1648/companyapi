public class CreateSubAdminModel
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; } // Rol adını dinamik olarak belirleyebilir
}
