using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public Priority Priority { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int? CreatedByAppUserId { get; set; }

    [ForeignKey(nameof(CreatedByAppUserId))]
    public AppUser? CreatedBy { get; set; }
}
