using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.ViewModels;

public class TaskFormViewModel
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public Priority Priority { get; set; }

    [Required]
    public int UserId { get; set; }

    public IEnumerable<SelectListItem> UserOptions { get; set; } = [];
}
