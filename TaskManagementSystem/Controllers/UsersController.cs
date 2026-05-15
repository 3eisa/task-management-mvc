using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers;

public class UsersController : BaseController
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db) => _db = db;

    // GET /Users
    public IActionResult Index() =>
        View(_db.Users.OrderBy(u => u.Name).ToList());

    // GET /Users/Create
    public IActionResult Create() => View();

    // POST /Users/Create
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(User user)
    {
        if (!ModelState.IsValid) return View(user);
        _db.Users.Add(user);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // GET /Users/Edit/5
    public IActionResult Edit(int id)
    {
        var user = _db.Users.Find(id);
        if (user is null) return NotFound();
        return View(user);
    }

    // POST /Users/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(int id, User user)
    {
        if (id != user.Id) return NotFound();
        if (!ModelState.IsValid) return View(user);
        _db.Users.Update(user);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // GET /Users/Delete/5
    public IActionResult Delete(int id)
    {
        var user = _db.Users.Find(id);
        if (user is null) return NotFound();
        return View(user);
    }

    // POST /Users/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var user = _db.Users.Find(id);
        if (user is null) return NotFound();
        _db.Users.Remove(user);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
