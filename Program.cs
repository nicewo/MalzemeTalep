using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using MalzemeTalep.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<MalzemeTalep.Services.PdfService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=malzemetalep.db"));
QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapGet("/sample.csv", () => Results.File(System.Text.Encoding.UTF8.GetBytes("Marka;Kategori;ModelKodu;Aciklama;TeknikOzellik;Birim\r\nSIEMENS;Otomat;5SL6110-7YA;1P C10A otomatik sigorta;230V 10A;ADET\r\n"), "text/csv; charset=utf-8", "malzeme_ornek.csv"));
app.UseRouting();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
