using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models;

// Dummy authentication only. Not a real password store.
public class AppUser
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}
