using System.ComponentModel.DataAnnotations;

public class CreateSubUserModel
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = default!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "Company ID is required")]
    public int CompanyId { get; set; } // Şirket ID'si
}
