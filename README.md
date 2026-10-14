# MalzemeTalep — ilk çalışan sürüm

ASP.NET Core 10 MVC + EF Core + SQLite tabanlı, internet üzerinden erişilebilen malzeme talep uygulaması başlangıç projesi.

## İçerik
- Malzeme kataloğu: marka, kategori, model/kod, açıklama, teknik özellik, birim.
- Elle malzeme ekleme/düzenleme/silme.
- XLSX ve CSV toplu malzeme içe aktarma.
- Proje ve malzeme talebi oluşturma.
- Talepleri katalogdan seçerek kalem ekleme.
- Talep detayından PDF çıktısı: mevcut form görünümüne yakın veya modern görünüm.
- Eski taleplerde ürün adı/kodu/açıklaması/miktar/birim anlık görüntü olarak saklanır.

## Gereksinimler
- .NET 10 SDK
- İlk çalıştırmada NuGet paketlerini indirmek için internet bağlantısı

## Çalıştırma
```bash
dotnet restore
dotnet run
```
Konsolda gösterilen `http://localhost:xxxx` adresini açın. Veritabanı `malzemetalep.db` olarak uygulama klasöründe oluşturulur.

## Toplu aktarım
Malzemeler > Excel/CSV içe aktar. Excel için `.xlsx`, CSV için UTF-8 veya Windows-1254 uyumlu virgül/noktalı virgül ayrımlı dosya kullanın.
Sütun başlıkları:
`Marka, Kategori, ModelKodu, Aciklama, TeknikOzellik, Birim`
ModelKodu ve Aciklama alanları zorunludur. Aynı ModelKodu varsa mevcut malzeme güncellenir.

## İnternetten erişim — önemli
Bu proje ilk çalışan uygulama iskeletidir; doğrudan internete açılmadan önce kimlik doğrulama, kullanıcı/rol yönetimi, HTTPS, güvenli yedekleme ve erişim politikası eklenmelidir. Bu sürümde giriş ekranı yoktur; yalnızca yerel test veya güvenli, kapalı test ortamı için kullanın. Üretimde herkese açık yayınlamayın.
Önerilen dağıtım: Linux VPS veya Windows Server + HTTPS reverse proxy + kalıcı disk/veritabanı yedeği. Birden fazla eşzamanlı kullanıcı ve yüksek veri hacminde SQLite yerine SQL Server/PostgreSQL değerlendirilebilir.

## PDF
QuestPDF Community lisans koşullarını kontrol edin: https://www.questpdf.com/license/
İlk sürümde iki ayrı çıktı seçeneği sunulur: `Kurumsal` (orijinal talep formunun düzenine yakın) ve `Modern`.
