# ✅ XAMPP İçin Çözüm Uygulandı

## 🔧 Yapılan Değişiklikler

### 1. `appsettings.json` Güncellendi

**Eski:**
```json
"DefaultConnection":"Server=localhost;Port=3306;Database=envanterdb;User=root;Password=root;"
```

**Yeni:**
```json
"DefaultConnection":"Server=localhost;Port=3306;Database=envanterdb;User=root;Password=;"
```

XAMPP'te MySQL root kullanıcısının varsayılan şifresi **boş** olduğu için `Password=;` olarak değiştirildi.

### 2. `Data/EnvanterContextFactory.cs` Güncellendi

Migration komutları için de aynı değişiklik yapıldı.

## 🚀 Şimdi Ne Yapmalısınız?

### 1. XAMPP'i Başlatın

- XAMPP Control Panel'i açın
- **MySQL** servisini başlatın (Start)

### 2. Veritabanını Kontrol Edin

phpMyAdmin'de (http://localhost/phpmyadmin) `envanterdb` veritabanının var olduğunu kontrol edin.

Yoksa oluşturun:
```sql
CREATE DATABASE envanterdb CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
```

### 3. Uygulamayı Çalıştırın

PowerShell'de:
```bash
dotnet run
```

## ✅ Beklenen Sonuç

Uygulama şu şekilde çalışmalı:

```
Building...
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand...
      CREATE TABLE `Kategoriler` ...
      CREATE TABLE `Konumlar` ...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5157
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

Tarayıcıda açın: http://localhost:5157

## 🔍 Hala Hata Alıyorsanız

### Senaryo 1: "Access denied" hatası devam ediyor

**Çözüm A:** XAMPP MySQL'inizde root şifresi varsa

`appsettings.json` dosyasında şifrenizi yazın:
```json
"DefaultConnection":"Server=localhost;Port=3306;Database=envanterdb;User=root;Password=SIZIN_SIFRENIZ;"
```

**Çözüm B:** Root şifresini sıfırlayın

1. XAMPP MySQL Shell'i açın:
```bash
mysql -u root
```

2. Şu komutları çalıştırın:
```sql
ALTER USER 'root'@'localhost' IDENTIFIED BY '';
FLUSH PRIVILEGES;
EXIT;
```

3. XAMPP'te MySQL'i yeniden başlatın

### Senaryo 2: "Unknown database" hatası

Veritabanı oluşturmayı unuttunuz:

```bash
mysql -u root
CREATE DATABASE envanterdb;
EXIT;
```

### Senaryo 3: "Can't connect to MySQL server"

XAMPP MySQL servisi çalışmıyor:

1. XAMPP Control Panel'i açın
2. MySQL'in yanındaki **Start** butonuna tıklayın
3. Yeşil arka plan görünmeli

## 📖 Detaylı Dokümantasyon

Tüm detaylar için: [XAMPP_SETUP.md](XAMPP_SETUP.md)

## 🎯 Özet

1. ✅ `appsettings.json` XAMPP için yapılandırıldı (şifre boş)
2. ✅ `EnvanterContextFactory.cs` güncellendi
3. ✅ XAMPP_SETUP.md oluşturuldu
4. ✅ README.md XAMPP desteği eklendi

**Şimdi sadece `dotnet run` komutunu çalıştırın!**

---

*Sorun devam ederse [TROUBLESHOOTING.md](TROUBLESHOOTING.md) dosyasına bakın veya hata mesajını tam olarak paylaşın.*
