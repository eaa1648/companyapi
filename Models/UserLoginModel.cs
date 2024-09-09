using System.ComponentModel.DataAnnotations;

public class UserLoginModel
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = default!;
}
