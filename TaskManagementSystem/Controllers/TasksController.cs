using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.ViewModels;

namespace TaskManagementSystem.Controllers;

public class TasksController : BaseController
{
    private readonly AppDbContext _db;

    public TasksController(AppDbContext db) => _db = db;

    // GET /Tasks
    public IActionResult Index(string? sortBy)
    {
        var selectedSort = (sortBy ?? "date_desc").ToLowerInvariant();
        IQueryable<TaskItem> query = _db.TaskItems
            .Include(t => t.User)
            .Include(t => t.CreatedBy);

        switch (selectedSort)
        {
            case "date_asc":
                query = query.OrderBy(t => t.CreatedAt);
                break;
            case "priority_asc":
                query = query.OrderBy(t => t.Priority).ThenByDescending(t => t.CreatedAt);
                break;
            case "priority_desc":
                query = query.OrderByDescending(t => t.Priority).ThenByDescending(t => t.CreatedAt);
                break;
            case "date_desc":
                query = query.OrderByDescending(t => t.CreatedAt);
                break;
            default:
                selectedSort = "date_desc";
                query = query.OrderByDescending(t => t.CreatedAt);
                break;
        }

        ViewData["SortBy"] = selectedSort;
        ViewData["CurrentAppUserId"] = CurrentAppUserId;
        return View(query.ToList());
    }

    // GET /Tasks/Create
    public IActionResult Create() => View(BuildForm(new TaskFormViewModel()));

    // POST /Tasks/Create
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(TaskFormViewModel model)
    {
        if (!ModelState.IsValid) return View(BuildForm(model));
        _db.TaskItems.Add(new TaskItem
        {
            Title              = model.Title,
            Description        = model.Description,
            Priority           = model.Priority,
            UserId             = model.UserId,
            CreatedAt          = DateTime.Now,
            CreatedByAppUserId = CurrentAppUserId
        });
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // GET /Tasks/Edit/5
    public IActionResult Edit(int id)
    {
        var task = _db.TaskItems.Find(id);
        if (task is null) return NotFound();
        if (task.CreatedByAppUserId != CurrentAppUserId) return RedirectToAction(nameof(Index));
        return View(BuildForm(new TaskFormViewModel
        {
            Id          = task.Id,
            Title       = task.Title,
            Description = task.Description,
            Priority    = task.Priority,
            UserId      = task.UserId
        }));
    }

    // POST /Tasks/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(int id, TaskFormViewModel model)
    {
        if (id != model.Id) return NotFound();
        if (!ModelState.IsValid) return View(BuildForm(model));
        var task = _db.TaskItems.Find(id);
        if (task is null) return NotFound();
        if (task.CreatedByAppUserId != CurrentAppUserId) return RedirectToAction(nameof(Index));
        task.Title       = model.Title;
        task.Description = model.Description;
        task.Priority    = model.Priority;
        task.UserId      = model.UserId;
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // GET /Tasks/Delete/5
    public IActionResult Delete(int id)
    {
        var task = _db.TaskItems.Include(t => t.User).FirstOrDefault(t => t.Id == id);
        if (task is null) return NotFound();
        if (task.CreatedByAppUserId != CurrentAppUserId) return RedirectToAction(nameof(Index));
        return View(task);
    }

    // POST /Tasks/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var task = _db.TaskItems.Find(id);
        if (task is null) return NotFound();
        if (task.CreatedByAppUserId != CurrentAppUserId) return RedirectToAction(nameof(Index));
        _db.TaskItems.Remove(task);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private TaskFormViewModel BuildForm(TaskFormViewModel model)
    {
        model.UserOptions = _db.Users
            .OrderBy(u => u.Name)
            .Select(u => new SelectListItem(u.Name, u.Id.ToString()))
            .ToList();
        return model;
    }
}
