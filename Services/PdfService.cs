using MalzemeTalep.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MalzemeTalep.Services;

public class PdfService
{
    public byte[] Create(MaterialRequest request, bool modern) =>
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(modern ? 32 : 24);
                page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(modern ? 9 : 8));
                page.Header().Column(header =>
                {
                    if (modern)
                    {
                        header.Item().Text("DATA OTOMASYON").FontSize(10).SemiBold().FontColor("#64748b");
                        header.Item().PaddingTop(6).Text("MALZEME TALEP FORMU").FontSize(22).Bold().FontColor("#0f172a");
                        header.Item().Text(request.Title).FontSize(13).SemiBold().FontColor("#2563eb");
                    }
                    else
                    {
                        header.Item().AlignCenter().Text("DATA OTOMASYON ELEKTRİK SANAYİ VE TİCARET LTD. ŞTİ.").Bold().FontSize(10);
                        header.Item().PaddingTop(4).AlignCenter().Text("MALZEME TALEP FORMU").Bold().FontSize(16);
                        header.Item().PaddingTop(3).Text($"LİSTE ADI: {request.Title}").Bold().FontSize(11);
                    }
                    header.Item().PaddingTop(5).Text($"Proje: {request.ProjectRecord?.Name ?? "-"}    Müşteri: {request.ProjectRecord?.Customer ?? "-"}");
                    header.Item().Text($"Tarih: {request.RequestDate:dd.MM.yyyy}");
                    header.Item().PaddingBottom(8).LineHorizontal(1).LineColor(modern ? "#cbd5e1" : "#111827");
                });
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(26);
                        columns.RelativeColumn(1.2f);
                        columns.RelativeColumn(1.5f);
                        columns.RelativeColumn(3.2f);
                        columns.ConstantColumn(48);
                        columns.ConstantColumn(45);
                    });
                    table.Header(h =>
                    {
                        foreach (var title in new[] { "SIRA", "MARKA", "ÜRÜN KODU", "AÇIKLAMA", "MİKTAR", "BİRİM" })
                        {
                            h.Cell().Background(modern ? "#1e293b" : "#e5e7eb").Padding(5).Text(title).Bold().FontColor(modern ? "#ffffff" : "#111827");
                        }
                    });
                    var index = 0;
                    foreach (var item in request.Items)
                    {
                        index++;
                        var bg = modern && index % 2 == 0 ? "#f1f5f9" : "#ffffff";
                        table.Cell().Background(bg).BorderBottom(0.5f).BorderColor("#cbd5e1").Padding(4).Text(index.ToString());
                        table.Cell().Background(bg).BorderBottom(0.5f).BorderColor("#cbd5e1").Padding(4).Text(item.BrandSnapshot);
                        table.Cell().Background(bg).BorderBottom(0.5f).BorderColor("#cbd5e1").Padding(4).Text(item.ModelCodeSnapshot);
                        table.Cell().Background(bg).BorderBottom(0.5f).BorderColor("#cbd5e1").Padding(4).Text(item.DescriptionSnapshot);
                        table.Cell().Background(bg).BorderBottom(0.5f).BorderColor("#cbd5e1").Padding(4).AlignRight().Text(item.Quantity.ToString("0.###"));
                        table.Cell().Background(bg).BorderBottom(0.5f).BorderColor("#cbd5e1").Padding(4).Text(item.UnitSnapshot);
                    }
                });
                page.Footer().AlignCenter().Text($"Malzeme talep kaydı #{request.Id}").FontSize(8).FontColor("#64748b");
            });
        }).GeneratePdf();
}
