# Kurulum ve Çalıştırma Rehberi

## Gereksinimler

- .NET 8.0 SDK
- Docker (MySQL için)
- veya MySQL 8.0+ (direkt kurulum)

## Hızlı Başlangıç

### 1. MySQL Veritabanını Başlat

#### Docker ile (Önerilen):

```bash
docker run -d \
  --name mysql-dev \
  -e MYSQL_ROOT_PASSWORD=root \
  -e MYSQL_DATABASE=envanterdb \
  -p 3306:3306 \
  mysql:8.0
```

#### Manuel MySQL Kurulumu:

MySQL'i sisteminize kurun ve bir veritabanı oluşturun:

```sql
CREATE DATABASE envanterdb;
```

### 2. Bağlantı Ayarları

`appsettings.json` dosyasını kontrol edin ve gerekirse güncelleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=envanterdb;User=root;Password=root;"
  }
}
```

**Veya** environment variable kullanın:

```bash
export DATABASE_URL="mysql://root:root@localhost:3306/envanterdb"
```

### 3. Uygulamayı Çalıştır

```bash
dotnet run
```

Uygulama otomatik olarak:
- Migration'ları uygulayacak
- Örnek verileri yükleyecek (10 kategori, 5 konum)
- Sunucuyu başlatacak (varsayılan: http://localhost:5xxx)

### 4. Tarayıcıda Aç

Konsol çıktısında gösterilen URL'yi tarayıcınızda açın (örn: http://localhost:5157)

## Örnek Veriler

Uygulama ilk çalıştığında otomatik olarak şu veriler yüklenir:

### Kategoriler
1. Hastane (kırmızı)
2. Banka (mavi)
3. Okul (yeşil)
4. Park (turkuaz)
5. Resmi Kurum (gri)
6. Alışveriş (turuncu)
7. Restoran (sarı)
8. Otopark (mor)
9. Kültür Merkezi (pembe)
10. Spor Tesisi (açık mavi)

### Örnek Konumlar
1. Sakarya Üniversitesi Eğitim ve Araştırma Hastanesi
2. Sakarya Büyükşehir Belediyesi
3. Kent Meydanı
4. Sakarya Park AVM
5. Adapazarı Müzesi

## Admin Girişi

Admin özellikleri için öncelikle bir kullanıcı oluşturmalısınız. Aşağıdaki SQL komutunu çalıştırabilirsiniz:

```sql
-- PostgreSQL veritabanına bağlanın ve çalıştırın:
INSERT INTO "AppUsers" ("KullaniciAdi", "Sifre", "Rol", "AktifMi")
VALUES ('admin', 'admin123', 'Admin', true);
```

Ardından `/Account/Login` sayfasından giriş yapın:
- Kullanıcı Adı: `admin`
- Şifre: `admin123`

⚠️ **Güvenlik Notu**: Üretim ortamında mutlaka güçlü şifreler kullanın ve şifreleri hash'leyin!

## Geliştirme

### Migration Oluşturma

```bash
dotnet ef migrations add YeniMigration
```

### Migration Uygulama

```bash
dotnet ef database update
```

### Veritabanını Sıfırlama

```bash
# Docker kullanıyorsanız:
docker stop postgres-dev
docker rm postgres-dev
# Ardından yukarıdaki "docker run" komutunu tekrar çalıştırın

# Manuel PostgreSQL:
DROP DATABASE envanterdb;
CREATE DATABASE envanterdb;
```

## Docker ile Tam Uygulama

Uygulamanın kendisini de Docker'da çalıştırmak için:

```bash
# MySQL başlat
docker run -d --name mysql-dev \
  -e MYSQL_ROOT_PASSWORD=root \
  -e MYSQL_DATABASE=envanterdb \
  mysql:8.0

# Uygulamayı derle ve çalıştır
docker build -t sakarya-rehber .
docker run -d -p 8080:80 \
  --link mysql-dev \
  -e DATABASE_URL="mysql://root:root@mysql-dev:3306/envanterdb" \
  sakarya-rehber
```

Tarayıcıda: http://localhost:8080

## Sorun Giderme

### "No such host is known" Hatası

MySQL sunucusuna erişilemiyor. Kontrol edin:

```bash
# MySQL container çalışıyor mu?
docker ps | grep mysql

# Port dinleniyor mu?
netstat -an | grep 3306
```

### Migration Hataları

Veritabanını sıfırlayın ve tekrar deneyin:

```bash
docker restart mysql-dev
dotnet run
```

### Port Çakışması

Eğer 3306 portu kullanılıyorsa, farklı bir port kullanın:

```bash
docker run -d --name mysql-dev \
  -e MYSQL_ROOT_PASSWORD=root \
  -e MYSQL_DATABASE=envanterdb \
  -p 3307:3306 \
  mysql:8.0
```

Ve `appsettings.json`'da port'u güncelleyin: `Port=3307`

## Üretim Ortamına Dağıtım

Üretim ortamı için:

1. Güvenli şifreler kullanın
2. `DATABASE_URL` environment variable'ı ayarlayın
3. SSL/TLS bağlantısı zorunlu kılın
4. AppUser şifrelerini hash'leyin (bcrypt, PBKDF2, vb.)
5. HTTPS kullanın
6. CORS ayarlarını yapılandırın

Örnek üretim connection string:

```
Server=production-host.com;Port=3306;Database=envanterdb;User=user;Password=secure_pass;SslMode=Required;
```

## Daha Fazla Bilgi

- [README.md](README.md) - Proje genel bilgileri
- [SAKARYA_REHBER_FEATURES.md](SAKARYA_REHBER_FEATURES.md) - Özellikler detayları
