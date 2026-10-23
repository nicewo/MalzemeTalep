# MalzemeTalep

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/)
[![Database](https://img.shields.io/badge/database-SQLite-003B57?logo=sqlite&logoColor=white)](https://www.sqlite.org/)
[![Docker](https://img.shields.io/badge/Docker-supported-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)

ASP.NET Core MVC ile geliştirilen web tabanlı malzeme kataloğu ve talep yönetimi uygulaması. Malzemeleri katalogda tutmayı, proje bazlı talepler oluşturmayı ve talepleri PDF olarak dışa aktarmayı amaçlar.

## Özellikler

- Malzeme kataloğu: marka, kategori, model/kod, açıklama, teknik özellik ve birim
- Malzeme ekleme, düzenleme ve silme
- Excel (.xlsx) ve CSV üzerinden toplu içe aktarma
- Proje kaydı ve proje bazlı malzeme talebi
- Katalogdan seçilen kalemlerle talep oluşturma
- Talep detayından Kurumsal veya Modern PDF çıktısı
- Eski taleplerde ürün bilgilerini ve miktarı kayıt anındaki görüntüsüyle saklama
- SQLite ile yerel veri saklama

## Teknoloji

- .NET 10 / ASP.NET Core MVC
- Entity Framework Core
- SQLite
- ClosedXML — Excel içe/dışa aktarma altyapısı
- QuestPDF — PDF üretimi
- Docker

## Gereksinimler

- .NET 10 SDK
- İlk derleme sırasında NuGet paketlerine erişim

## Yerel çalıştırma

```bash
git clone https://github.com/nicewo/MalzemeTalep.git
cd MalzemeTalep
dotnet restore
dotnet run
```

Konsolda yazdırılan yerel adresi tarayıcıda açın. Varsayılan veritabanı bağlantısı `Data Source=malzemetalep.db` şeklindedir; uygulama veritabanını ilk başlangıçta oluşturur.

## Docker

Proje kök dizininde:

```bash
docker build -t malzemetalep .
docker run --rm -p 10000:10000 -v malzemetalep-data:/app malzemetalep
```

Ardından `http://localhost:10000` adresini açın. Kalıcı veri için kullanılan volume ve veritabanı dosyasının yedeklenmesini doğrulayın.

## Excel/CSV içe aktarma

Kullanılan sütun başlıkları:

`Marka, Kategori, ModelKodu, Aciklama, TeknikOzellik, Birim`

- Model kodu ve açıklama alanları zorunludur.
- Aynı model kodu bulunduğunda mevcut kayıt güncellenir.
- CSV dosyalarında UTF-8 veya Windows-1254 kodlaması ve virgül/noktalı virgül ayırıcıları kullanılabilir.

## Güvenlik ve dağıtım

> **Önemli:** Mevcut sürümde kimlik doğrulama ve kullanıcı/rol yönetimi yoktur. Uygulamayı doğrudan internete açık üretim ortamında yayımlamayın.

İnternetten erişim planlanıyorsa kimlik doğrulama, yetkilendirme, HTTPS, güvenli yedekleme, loglama ve erişim politikaları eklenip test edilmelidir. Hassas veya ticari verileri gerçek kullanıcılarla kullanmadan önce bu kontroller tamamlanmalıdır.

## PDF lisansı

QuestPDF Community lisans koşullarını dağıtım modelinize göre inceleyin: https://www.questpdf.com/license/
