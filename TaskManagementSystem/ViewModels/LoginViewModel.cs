using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.ViewModels;

public class LoginViewModel
{
    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(100), DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
