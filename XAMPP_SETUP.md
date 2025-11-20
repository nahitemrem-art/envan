# XAMPP ile Kurulum Rehberi

## ✅ Önkoşullar

- XAMPP kurulu olmalı
- .NET 8.0 SDK kurulu olmalı

## 📝 Adım Adım Kurulum

### 1. XAMPP'i Başlat

1. XAMPP Control Panel'i açın
2. **MySQL** modülünü başlatın (Start butonuna tıklayın)
3. MySQL'in çalıştığını kontrol edin (yeşil arka plan)

### 2. Veritabanı Oluştur

**Seçenek 1: phpMyAdmin ile**

1. XAMPP Control Panel'de MySQL yanındaki **Admin** butonuna tıklayın
2. phpMyAdmin açılacak
3. Üst menüden **"Databases"** (Veritabanları) sekmesine tıklayın
4. "Create database" bölümüne `envanterdb` yazın
5. Collation: `utf8mb4_general_ci` seçin
6. **Create** butonuna tıklayın

**Seçenek 2: MySQL Console ile**

1. XAMPP Control Panel'de MySQL yanındaki **Shell** butonuna tıklayın
2. Şu komutları çalıştırın:

```bash
mysql -u root
```

```sql
CREATE DATABASE envanterdb CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
SHOW DATABASES;
EXIT;
```

### 3. Bağlantı Ayarlarını Kontrol Et

`appsettings.json` dosyası zaten XAMPP için yapılandırılmış durumda:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=envanterdb;User=root;Password=;"
  }
}
```

**NOT:** XAMPP'te MySQL root kullanıcısının varsayılan şifresi **boş**tur. `Password=;` bu yüzden boş.

### 4. Uygulamayı Çalıştır

PowerShell veya CMD'de proje klasöründe:

```bash
dotnet run
```

Uygulama otomatik olarak:
- ✅ Veritabanı tablolarını oluşturacak (migration)
- ✅ Örnek verileri ekleyecek (10 kategori, 5 konum)
- ✅ Web sunucusunu başlatacak

### 5. Tarayıcıda Aç

Konsol çıktısında gösterilen URL'yi açın:
```
http://localhost:5157
```

## 🔧 Sorun Giderme

### Hata: "Access denied for user 'root'@'localhost'"

**Neden:** MySQL root kullanıcısının şifresi boş değil.

**Çözüm 1:** Şifrenizi öğrenin ve `appsettings.json`'da güncelleyin

XAMPP MySQL şifreniz varsa (örneğin "mypassword"):

```json
"DefaultConnection": "Server=localhost;Port=3306;Database=envanterdb;User=root;Password=mypassword;"
```

**Çözüm 2:** Root şifresini sıfırlayın

1. XAMPP MySQL'i durdurun
2. XAMPP Shell'i açın
3. Şu komutları çalıştırın:

```bash
cd C:\xampp\mysql\bin
mysql -u root
```

```sql
ALTER USER 'root'@'localhost' IDENTIFIED BY '';
FLUSH PRIVILEGES;
EXIT;
```

4. XAMPP MySQL'i yeniden başlatın

### Hata: "Port 3306 already in use"

**Çözüm:**
1. XAMPP Control Panel'de MySQL'in başladığından emin olun
2. Başka bir MySQL servisi çalışmıyorsa kontrol edin (Windows Services)

### Hata: "Unknown database 'envanterdb'"

**Çözüm:** Veritabanını oluşturun

```bash
cd C:\xampp\mysql\bin
mysql -u root -e "CREATE DATABASE envanterdb;"
```

### Hata: Migration hataları

**Çözüm 1:** Migration'ları elle uygula

```bash
dotnet ef database update
```

**Çözüm 2:** Veritabanını sıfırla ve yeniden oluştur

```bash
# MySQL Shell'de
mysql -u root
DROP DATABASE IF EXISTS envanterdb;
CREATE DATABASE envanterdb;
EXIT;

# Sonra uygulamayı çalıştır
dotnet run
```

## 📊 Veri Kontrolü

### phpMyAdmin ile

1. phpMyAdmin'i açın (http://localhost/phpmyadmin)
2. Sol panelden `envanterdb` seçin
3. Tablolar görünmeli:
   - AppUsers
   - Cihazlar
   - Kategoriler
   - Konumlar
   - Personeller
   - Zimmetler
   - __EFMigrationsHistory

### MySQL Console ile

```bash
cd C:\xampp\mysql\bin
mysql -u root envanterdb
```

```sql
-- Tabloları listele
SHOW TABLES;

-- Kategori sayısı (10 olmalı)
SELECT COUNT(*) FROM Kategoriler;

-- Konum sayısı (5 olmalı)
SELECT COUNT(*) FROM Konumlar;

-- Konumları listele
SELECT K.Ad, Kat.Ad as Kategori FROM Konumlar K
JOIN Kategoriler Kat ON K.KategoriId = Kat.Id;
```

## 🎯 Admin Kullanıcısı Ekleme (Opsiyonel)

Admin özelliklerini kullanmak için:

```bash
cd C:\xampp\mysql\bin
mysql -u root envanterdb
```

```sql
INSERT INTO AppUsers (KullaniciAdi, Sifre, Rol, AktifMi)
VALUES ('admin', 'admin123', 'Admin', 1);

SELECT * FROM AppUsers;
EXIT;
```

Sonra `/Account/Login` sayfasından giriş yapın:
- Kullanıcı Adı: `admin`
- Şifre: `admin123`

## 🚀 XAMPP Kısayolları

### MySQL Başlat/Durdur

```bash
# Başlat
net start mysql

# Durdur
net stop mysql
```

### MySQL Log Kontrol

Log dosyası: `C:\xampp\mysql\data\mysql_error.log`

### MySQL Yapılandırma

Yapılandırma dosyası: `C:\xampp\mysql\bin\my.ini`

## ⚙️ XAMPP Ayarları

### MySQL Port Değiştirme (Opsiyonel)

Eğer 3306 portu kullanılıyorsa:

1. `C:\xampp\mysql\bin\my.ini` dosyasını açın
2. `port=3306` satırını bulun ve değiştirin (örn: `port=3307`)
3. MySQL'i yeniden başlatın
4. `appsettings.json`'da port'u güncelleyin:

```json
"DefaultConnection": "Server=localhost;Port=3307;Database=envanterdb;User=root;Password=;"
```

### Karakter Seti Ayarları

`my.ini` dosyasında şu ayarların olduğundan emin olun:

```ini
[mysqld]
character-set-server=utf8mb4
collation-server=utf8mb4_unicode_ci

[client]
default-character-set=utf8mb4
```

## 📚 Diğer Kaynaklar

- [README.md](README.md) - Proje genel bilgileri
- [QUICK_START.md](QUICK_START.md) - Hızlı başlangıç
- [TROUBLESHOOTING.md](TROUBLESHOOTING.md) - Sorun giderme

## ✅ Başarılı Kurulum Kontrolü

Aşağıdaki adımlar tamamlanmışsa kurulum başarılıdır:

- [x] XAMPP MySQL çalışıyor
- [x] `envanterdb` veritabanı oluşturuldu
- [x] `dotnet run` komutu hatasız çalışıyor
- [x] http://localhost:5157 açılıyor
- [x] Ana sayfada "Sakarya Kent Rehberi" yazısı görünüyor
- [x] `/Harita` sayfasında harita yükleniyor
- [x] phpMyAdmin'de tablolar ve veriler görünüyor

---

**Başarılar! 🎉**

Herhangi bir sorunla karşılaşırsanız [TROUBLESHOOTING.md](TROUBLESHOOTING.md) dosyasına bakın.
