using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MalzemeTalep.Data;

namespace MalzemeTalep.Controllers;

public class HomeController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.MaterialCount = await db.Materials.CountAsync();
        ViewBag.ProjectCount = await db.Projects.CountAsync();
        ViewBag.RequestCount = await db.Requests.CountAsync();
        ViewBag.Recent = await db.Requests.Include(x => x.ProjectRecord).OrderByDescending(x => x.Id).Take(8).ToListAsync();
        return View();
    }
}
