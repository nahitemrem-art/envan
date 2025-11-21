# Hızlı Başlangıç Rehberi - Sakarya Kent Rehberi

## 🚀 5 Dakikada Başlat

### 1. MySQL Container'ı Başlat

```bash
docker run -d \
  --name mysql-dev \
  -e MYSQL_ROOT_PASSWORD=root \
  -e MYSQL_DATABASE=envanterdb \
  -p 3306:3306 \
  mysql:8.0
```

**Bekleme:** MySQL'in hazır olması için 15-20 saniye bekleyin.

### 2. Uygulamayı Başlat

```bash
cd /home/engine/project
dotnet run
```

Uygulama otomatik olarak:
- ✅ Migration'ları uygular
- ✅ 10 kategori yükler
- ✅ 5 örnek konum ekler
- ✅ Web sunucusunu başlatır

### 3. Tarayıcıda Aç

Konsol çıktısında gösterilen URL'yi açın:
```
http://localhost:5157
```

## 📱 Sayfalar

| URL | Açıklama | Yetki |
|-----|----------|-------|
| `/` | Ana sayfa | Herkes |
| `/Harita` | İnteraktif harita | Herkes |
| `/Harita/Kent` | Kent rehberi (alternatif) | Herkes |
| `/Konum` | Konum yönetimi | Admin |
| `/Kategori` | Kategori yönetimi | Admin |

## 🔑 Admin Girişi (Opsiyonel)

Admin özelliklerini kullanmak için bir kullanıcı oluşturun:

```bash
docker exec -it mysql-dev mysql -uroot -proot envanterdb
```

```sql
INSERT INTO AppUsers (KullaniciAdi, Sifre, Rol, AktifMi)
VALUES ('admin', 'admin123', 'Admin', true);
```

Sonra `/Account/Login` sayfasından giriş yapın.

## 🛑 Durdurma

```bash
# Uygulamayı durdur (Ctrl+C veya)
pkill -f "dotnet run"

# MySQL'i durdur (isteğe bağlı)
docker stop mysql-dev
```

## 🔄 Yeniden Başlatma

```bash
# MySQL'i tekrar başlat
docker start mysql-dev

# Uygulamayı başlat
dotnet run
```

## ✅ Kontrol

### MySQL Çalışıyor mu?

```bash
docker ps | grep mysql
```

Çıktı görmelisiniz:
```
b8325134ca90   mysql:8.0   ...   Up X minutes   0.0.0.0:3306->3306/tcp
```

### Veritabanı Bağlantısı Test

```bash
docker exec mysql-dev mysql -uroot -proot -e "SHOW DATABASES;"
```

### Veri Kontrolü

```bash
docker exec mysql-dev mysql -uroot -proot -e "USE envanterdb; SELECT COUNT(*) FROM Konumlar;"
```

## 🐛 Sorun Giderme

### Hata: "Unable to connect to MySQL"

**Çözüm:** MySQL container'ı çalışmıyor.

```bash
docker start mysql-dev
# veya yeniden oluştur
docker rm mysql-dev
docker run -d --name mysql-dev -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=envanterdb -p 3306:3306 mysql:8.0
```

### Hata: "Port 3306 already in use"

**Çözüm:** Farklı bir port kullanın.

```bash
docker run -d --name mysql-dev -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=envanterdb -p 3307:3306 mysql:8.0
```

`appsettings.json`'da port'u güncelleyin: `Port=3307`

### Hata: "Migration failed"

**Çözüm:** Veritabanını sıfırlayın.

```bash
docker exec -it mysql-dev mysql -uroot -proot -e "DROP DATABASE IF EXISTS envanterdb; CREATE DATABASE envanterdb;"
dotnet run
```

### Uygulama Başlamıyor

**Kontroller:**
1. .NET 8 SDK kurulu mu? `dotnet --version`
2. Port 5157 kullanımda mı? `netstat -an | grep 5157`
3. Packages restore edildi mi? `dotnet restore`

## 📊 Örnek Veriler

Uygulama ilk çalıştığında otomatik olarak yüklenir:

**10 Kategori:**
- Hastane, Banka, Okul, Park, Resmi Kurum
- Alışveriş, Restoran, Otopark, Kültür Merkezi, Spor Tesisi

**5 Konum:**
- Sakarya Üniversitesi Eğitim ve Araştırma Hastanesi
- Sakarya Büyükşehir Belediyesi
- Kent Meydanı
- Sakarya Park AVM
- Adapazarı Müzesi

## 🎨 Özellikler

- ✅ İnteraktif harita (Leaflet + OpenStreetMap)
- ✅ Kategori bazlı filtreleme
- ✅ Arama fonksiyonu
- ✅ Responsive tasarım (mobil uyumlu)
- ✅ Admin CRUD işlemleri
- ✅ GPS koordinat desteği
- ✅ Bootstrap 5 + Bootstrap Icons

## 📚 Daha Fazla Bilgi

- [SETUP.md](SETUP.md) - Detaylı kurulum
- [README.md](README.md) - Proje genel bilgileri
- [MYSQL_MIGRATION.md](MYSQL_MIGRATION.md) - PostgreSQL'den MySQL'e geçiş
- [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md) - Geliştirme özeti

## 🆘 Yardım

Sorun yaşıyorsanız:

1. MySQL container'ını kontrol edin: `docker ps | grep mysql`
2. Logları inceleyin: `docker logs mysql-dev`
3. Uygulama build hatası: `dotnet build`
4. Test script çalıştırın: `./test-app.sh`

---

**İyi çalışmalar! 🚀**
