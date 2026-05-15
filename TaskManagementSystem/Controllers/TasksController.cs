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
    public IActionResult Index() =>
        View(_db.TaskItems.Include(t => t.User).OrderBy(t => t.CreatedAt).ToList());

    // GET /Tasks/Create
    public IActionResult Create() => View(BuildForm(new TaskFormViewModel()));

    // POST /Tasks/Create
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(TaskFormViewModel model)
    {
        if (!ModelState.IsValid) return View(BuildForm(model));
        _db.TaskItems.Add(new TaskItem
        {
            Title       = model.Title,
            Description = model.Description,
            Priority    = model.Priority,
            UserId      = model.UserId,
            CreatedAt   = DateTime.Now
        });
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // GET /Tasks/Edit/5
    public IActionResult Edit(int id)
    {
        var task = _db.TaskItems.Find(id);
        if (task is null) return NotFound();
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
        return View(task);
    }

    // POST /Tasks/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var task = _db.TaskItems.Find(id);
        if (task is null) return NotFound();
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
