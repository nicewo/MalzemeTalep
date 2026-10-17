using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MalzemeTalep.Data;
using MalzemeTalep.Models;
using System.Text;

namespace MalzemeTalep.Controllers;

public class MaterialsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? q)
    {
        var query = db.Materials.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(x => x.Brand.Contains(q) || x.Category.Contains(q) || x.ModelCode.Contains(q) || x.Description.Contains(q));
        ViewBag.Query = q;
        return View(await query.OrderBy(x => x.Brand).ThenBy(x => x.ModelCode).Take(1000).ToListAsync());
    }

    public IActionResult Create() => View("Edit", new Material());
    public async Task<IActionResult> Edit(int id) => View(await db.Materials.FindAsync(id) ?? new Material());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(Material model)
    {
        if (!ModelState.IsValid) return View("Edit", model);
        model.UpdatedAt = DateTime.UtcNow;
        if (model.Id == 0) db.Materials.Add(model);
        else db.Materials.Update(model);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.Materials.FindAsync(id);
        if (item != null) { db.Materials.Remove(item); await db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Import() => View();

    [HttpPost, ValidateAntiForgeryToken, RequestSizeLimit(20_000_000)]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file == null || file.Length == 0) { TempData["Error"] = "Dosya seçilmedi."; return View(); }
        var rows = new List<string[]>();
        try
        {
            if (Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                using var stream = file.OpenReadStream();
                using var workbook = new XLWorkbook(stream);
                var sheet = workbook.Worksheet(1);
                foreach (var row in sheet.RowsUsed())
                    rows.Add(row.Cells(1, 6).Select(c => c.GetString().Trim()).ToArray());
            }
            else if (Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8, true);
                string? line;
                while ((line = await reader.ReadLineAsync()) is not null)
                {
                    var semicolonCells = line.Split(';');
                    var commaCells = line.Split(',');
                    rows.Add(semicolonCells.Length >= commaCells.Length ? semicolonCells : commaCells);
                    rows[^1] = rows[^1].Select(x => x.Trim().Trim('"')).ToArray();
                }
            }
            else { TempData["Error"] = "Yalnızca .xlsx veya .csv dosyası desteklenir."; return View(); }

            var first = true; var added = 0; var updated = 0; var skipped = 0;
            foreach (var cells in rows)
            {
                if (first) { first = false; if (cells.Any(x => x.Contains("model", StringComparison.OrdinalIgnoreCase) || x.Contains("marka", StringComparison.OrdinalIgnoreCase))) continue; }
                string Get(int i) => i < cells.Length ? cells[i].Trim() : "";
                var code = Get(2); var description = Get(3);
                if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(description)) { skipped++; continue; }
                var material = await db.Materials.FirstOrDefaultAsync(x => x.ModelCode == code);
                if (material == null)
                {
                    db.Materials.Add(new Material { Brand = Get(0), Category = Get(1), ModelCode = code, Description = description, TechnicalSpecs = Get(4), Unit = string.IsNullOrWhiteSpace(Get(5)) ? "ADET" : Get(5), UpdatedAt = DateTime.UtcNow });
                    added++;
                }
                else
                {
                    material.Brand = Get(0); material.Category = Get(1); material.Description = description; material.TechnicalSpecs = Get(4); material.Unit = string.IsNullOrWhiteSpace(Get(5)) ? "ADET" : Get(5); material.UpdatedAt = DateTime.UtcNow;
                    updated++;
                }
            }
            await db.SaveChangesAsync();
            TempData["Message"] = $"Aktarım tamamlandı. Yeni: {added}, güncellenen: {updated}, atlanan: {skipped}.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { TempData["Error"] = "Dosya okunamadı: " + ex.Message; return View(); }
    }
}
