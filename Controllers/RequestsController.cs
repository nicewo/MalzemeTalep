using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MalzemeTalep.Data;
using MalzemeTalep.Models;
using MalzemeTalep.Services;

namespace MalzemeTalep.Controllers;

public class RequestsController(AppDbContext db, PdfService pdf) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Requests.Include(x => x.ProjectRecord).OrderByDescending(x => x.Id).ToListAsync());

    public async Task<IActionResult> Create()
    {
        ViewBag.Projects = await db.Projects.OrderBy(x => x.Name).ToListAsync();
        return View(new MaterialRequest { RequestDate = DateTime.Today });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MaterialRequest model)
    {
        if (string.IsNullOrWhiteSpace(model.Title)) ModelState.AddModelError(nameof(model.Title), "Talep adı zorunludur.");
        if (!ModelState.IsValid) { ViewBag.Projects = await db.Projects.ToListAsync(); return View(model); }
        model.Items = [];
        db.Requests.Add(model); await db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    public async Task<IActionResult> Details(int id)
    {
        var request = await db.Requests.Include(x => x.ProjectRecord).Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
        if (request == null) return NotFound();
        ViewBag.Materials = await db.Materials.OrderBy(x => x.Brand).ThenBy(x => x.ModelCode).ToListAsync();
        return View(request);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(int requestId, int materialId, decimal quantity)
    {
        var request = await db.Requests.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == requestId);
        var material = await db.Materials.FindAsync(materialId);
        if (request == null || material == null || quantity <= 0) return BadRequest();
        request.Items.Add(new RequestItem { MaterialId = material.Id, BrandSnapshot = material.Brand, ModelCodeSnapshot = material.ModelCode, DescriptionSnapshot = material.Description, SpecsSnapshot = material.TechnicalSpecs, UnitSnapshot = material.Unit, Quantity = quantity });
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = requestId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveItem(int requestId, int itemId)
    {
        var item = await db.RequestItems.FirstOrDefaultAsync(x => x.Id == itemId && x.RequestId == requestId);
        if (item != null) { db.RequestItems.Remove(item); await db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Details), new { id = requestId });
    }

    public async Task<IActionResult> Pdf(int id, string style = "corporate")
    {
        var request = await db.Requests.Include(x => x.ProjectRecord).Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
        if (request == null) return NotFound();
        var bytes = pdf.Create(request, style == "modern");
        return File(bytes, "application/pdf", $"MalzemeTalep_{id}_{style}.pdf");
    }
}
