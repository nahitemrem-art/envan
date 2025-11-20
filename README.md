# Sakarya Kent Rehberi

ASP.NET Core 8 MVC uygulaması - Envanter Takip Sistemi ve Sakarya Kent Rehberi

## Özellikler

### Kent Rehberi (Yeni!)
- 🗺️ İnteraktif harita görünümü (Leaflet.js)
- 📍 Kategorilere göre konumlar (Hastane, Banka, Okul, Park, vb.)
- 🔍 Gelişmiş arama ve filtreleme
- 📱 Responsive tasarım
- 📝 Detaylı konum bilgileri (adres, telefon, çalışma saatleri)

### Envanter Takip Sistemi
- Cihaz yönetimi
- Personel yönetimi
- Zimmet takibi
- Admin paneli

## Kurulum

### Gereksinimler
- .NET 8.0 SDK
- **XAMPP** (önerilen - MySQL içerir) VEYA
- Docker (MySQL için) VEYA MySQL 8.0+

### ⚡ Hızlı Başlangıç

**XAMPP ile (Windows - Önerilen):**

1. XAMPP'i başlatın ve MySQL'i çalıştırın
2. phpMyAdmin'de `envanterdb` veritabanını oluşturun
3. PowerShell'de:
```bash
dotnet run
```

**Detaylı XAMPP kurulumu için:** [XAMPP_SETUP.md](XAMPP_SETUP.md)

---

**Docker ile (Linux/Mac):**

```bash
# En kolay yol
./start.sh

# Veya manuel
docker run -d --name mysql-dev -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=envanterdb -p 3306:3306 mysql:8.0
sleep 20
dotnet run
```

Uygulama otomatik olarak migration'ları uygular ve örnek verileri yükler.

**📚 Dokümantasyon:**
- [XAMPP_SETUP.md](XAMPP_SETUP.md) - XAMPP ile kurulum (Windows) 💻
- [QUICK_START.md](QUICK_START.md) - 5 dakikada başlangıç (Docker) 🚀
- [SETUP.md](SETUP.md) - Detaylı kurulum 📖
- [TROUBLESHOOTING.md](TROUBLESHOOTING.md) - Sorun giderme 🔧
- [MYSQL_MIGRATION.md](MYSQL_MIGRATION.md) - PostgreSQL'den geçiş rehberi 🔄

## Kullanım

### Kent Rehberi

1. Ana sayfadan "Kent Rehberi" veya "Haritayı Görüntüle" butonuna tıklayın
2. Sol panelde konumları listeleyin, haritada görüntüleyin
3. Kategorilere göre filtreleyin
4. Arama yapın
5. Konum detaylarına tıklayarak daha fazla bilgi edinin

### Admin İşlemleri (Giriş Gerektirir)

#### Kategori Yönetimi
- `/Kategori/Index` - Kategorileri listele
- `/Kategori/Create` - Yeni kategori ekle
- Kategori rengi ve ikonu belirleyin

#### Konum Yönetimi
- `/Konum/Index` - Konumları listele
- `/Konum/Create` - Yeni konum ekle
- Koordinatları Google Maps'ten alın:
  1. Google Maps'i açın
  2. Konuma sağ tıklayın
  3. "Burası ne?" seçin
  4. Alt kısımda görünen koordinatları kopyalayın

## Teknolojiler

- ASP.NET Core 8 MVC
- Entity Framework Core
- MySQL (Pomelo.EntityFrameworkCore.MySql)
- Bootstrap 5
- Leaflet.js (OpenStreetMap)
- Bootstrap Icons

## Proje Yapısı

```
├── Controllers/
│   ├── HaritaController.cs     # Kent rehberi görünümleri
│   ├── KonumController.cs      # Konum CRUD işlemleri
│   ├── KategoriController.cs   # Kategori CRUD işlemleri
│   └── ...                     # Diğer controller'lar
├── Models/
│   ├── Konum.cs                # Konum entity
│   ├── Kategori.cs             # Kategori entity
│   └── ...                     # Diğer model'ler
├── Views/
│   ├── Harita/                 # Kent rehberi view'ları
│   ├── Konum/                  # Konum yönetim view'ları
│   ├── Kategori/               # Kategori yönetim view'ları
│   └── ...                     # Diğer view'lar
├── Data/
│   ├── EnvanterContext.cs      # DbContext
│   └── DbSeeder.cs             # Örnek veri seed'leme
└── Migrations/                 # EF Core migrations

```

## Docker Desteği

```bash
docker build -t sakarya-rehber .
docker run -p 8080:80 -e DATABASE_URL="mysql://root:root@mysql-dev:3306/envanterdb" sakarya-rehber
```

## Lisans

Bu proje örnek bir uygulamadır.
