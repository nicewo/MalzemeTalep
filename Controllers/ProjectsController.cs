using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MalzemeTalep.Data;
using MalzemeTalep.Models;

namespace MalzemeTalep.Controllers;

public class ProjectsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Projects.OrderByDescending(x => x.Id).ToListAsync());
    public IActionResult Create() => View(new ProjectRecord());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(ProjectRecord model)
    {
        if (!ModelState.IsValid) return View("Create", model);
        db.Projects.Add(model); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
}
