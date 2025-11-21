# 🔧 Entity Framework Migration Sorunları ve Çözümleri

## 🚨 Yaygın Sorunlar

### 1. "Unknown column" Hatası

**Hata:**
```
MySqlException: Unknown column 'k.EkleyenKullanici' in 'field list'
```

**Neden:** Migration dosyası oluşturulmuş ancak veritabanına uygulanmamış.

**Çözüm:**

#### A. Manuel SQL ile (ÖNERİLEN - Hızlı)

phpMyAdmin'de (http://localhost/phpmyadmin):
```sql
ALTER TABLE Konumlar ADD COLUMN EkleyenKullanici VARCHAR(100) NULL;
```

#### B. Migration History Düzelt

```sql
USE envanterdb;

-- Kolonu ekle
ALTER TABLE Konumlar ADD COLUMN IF NOT EXISTS EkleyenKullanici VARCHAR(100) NULL;

-- Migration history'e kaydet
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
VALUES ('20250120000000_AddEkleyenKullaniciToKonum', '8.0.11')
ON DUPLICATE KEY UPDATE ProductVersion = ProductVersion;
```

#### C. EF Core ile (Yavaş ama Güvenli)

```bash
# Veritabanını sil ve yeniden oluştur
dotnet ef database drop --force
dotnet ef database update
```

⚠️ **DİKKAT:** Bu komut tüm verileri siler!

---

### 2. Migration Uygulanmıyor

**Belirtiler:**
- `dotnet run` çalışıyor ama kolon eklenmemiş
- Program.cs'de `context.Database.Migrate()` var ama çalışmıyor

**Çözüm:**

#### Kontrol 1: Migration Dosyaları

```bash
ls -la Migrations/
```

Şunları görmelisiniz:
- `20250120000000_AddEkleyenKullaniciToKonum.cs`
- `20250120000000_AddEkleyenKullaniciToKonum.Designer.cs`

#### Kontrol 2: Migration History Tablosu

phpMyAdmin'de:
```sql
SELECT * FROM __EFMigrationsHistory ORDER BY MigrationId DESC;
```

Son migration `20250120000000_AddEkleyenKullaniciToKonum` olmalı.

Yoksa:
```sql
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
VALUES ('20250120000000_AddEkleyenKullaniciToKonum', '8.0.11');
```

#### Kontrol 3: Kolon Var mı?

```sql
DESCRIBE Konumlar;
```

veya

```sql
SHOW COLUMNS FROM Konumlar LIKE 'EkleyenKullanici';
```

Boş sonuç = Kolon yok, SQL ile ekleyin.

---

### 3. Pending Migrations (Bekleyen Migration'lar)

**Hata:**
```
There are pending model changes that haven't been applied to the database
```

**Çözüm:**

```bash
# Yeni migration oluştur
dotnet ef migrations add YourMigrationName

# Uygula
dotnet ef database update
```

---

### 4. Migration Çakışması

**Hata:**
```
A migration named 'AddEkleyenKullaniciToKonum' has already been applied to the database
```

**Çözüm:**

#### A. Migration'ı Geri Al

```bash
dotnet ef database update PreviousMigrationName
```

#### B. Migration'ı Tamamen Sil

```bash
# Migration dosyasını sil
rm Migrations/20250120000000_AddEkleyenKullaniciToKonum.cs
rm Migrations/20250120000000_AddEkleyenKullaniciToKonum.Designer.cs

# History'den sil
```

phpMyAdmin'de:
```sql
DELETE FROM __EFMigrationsHistory 
WHERE MigrationId = '20250120000000_AddEkleyenKullaniciToKonum';
```

#### C. Yeniden Oluştur

```bash
dotnet ef migrations add AddEkleyenKullaniciToKonum
dotnet ef database update
```

---

### 5. "Cannot connect to MySQL" Hatası

**Hata:**
```
Unable to connect to any of the specified MySQL hosts
```

**Çözüm:**

1. XAMPP MySQL'in çalıştığından emin olun
   - XAMPP Control Panel > MySQL > Start

2. Bağlantı bilgilerini kontrol edin:

`appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=envanterdb;User=root;Password=;"
  }
}
```

3. MySQL portunu kontrol edin:

```bash
netstat -ano | findstr :3306
```

Port kullanımda olmalı.

4. Veritabanının var olduğundan emin olun:

phpMyAdmin'de `envanterdb` veritabanını görmelisiniz.

Yoksa oluşturun:
```sql
CREATE DATABASE IF NOT EXISTS envanterdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

---

## 🛠️ Kullanışlı Komutlar

### Migration Oluştur
```bash
dotnet ef migrations add MigrationName
```

### Migration Uygula
```bash
dotnet ef database update
```

### Son Migration'ı Geri Al
```bash
dotnet ef database update PreviousMigrationName
```

### Tüm Migration'ları Listele
```bash
dotnet ef migrations list
```

### Migration SQL Scriptini Gör
```bash
dotnet ef migrations script
```

### Belirli Bir Migration'a Git
```bash
dotnet ef database update 20250120000000_AddEkleyenKullaniciToKonum
```

### Veritabanını Sıfırla (DİKKAT! Veriler silinir)
```bash
dotnet ef database drop --force
dotnet ef database update
```

---

## 🔍 Sorun Giderme Adımları

### Adım 1: Veritabanı Bağlantısı
```bash
# Test et
dotnet run
```

Bağlantı hatası varsa → MySQL çalışmıyor veya şifre yanlış.

### Adım 2: Migration History Kontrol
phpMyAdmin SQL:
```sql
SELECT * FROM __EFMigrationsHistory;
```

### Adım 3: Tablo Yapısını Kontrol
```sql
DESCRIBE Konumlar;
```

### Adım 4: Migration Dosyalarını Kontrol
```bash
ls -la Migrations/
```

### Adım 5: Cache Temizle ve Yeniden Build
```bash
dotnet clean
dotnet build
dotnet run
```

---

## 📝 En İyi Pratikler

1. **Migration oluşturmadan önce:**
   - Model değişikliklerini tamamlayın
   - Build alın: `dotnet build`
   - Migration oluşturun: `dotnet ef migrations add Name`

2. **Migration uygulamadan önce:**
   - Veritabanının yedeğini alın
   - Migration script'ini inceleyin: `dotnet ef migrations script`
   - Test ortamında deneyin

3. **Production'da:**
   - Migration'ları otomatik uygulamayın (Program.cs'deki `Migrate()` yoruma alın)
   - Manuel SQL script çalıştırın
   - Rollback planı hazırlayın

4. **Geliştirme sırasında:**
   - Sık sık küçük migration'lar oluşturun
   - Anlamlı isimler kullanın
   - Migration'ları git'e commit edin

---

## 🚀 Hızlı Çözüm Şablonu

Her zaman işe yarayan çözüm:

```bash
# 1. Veritabanını sıfırla
dotnet ef database drop --force

# 2. Migration'ları yeniden uygula
dotnet ef database update

# 3. Çalıştır
dotnet run
```

⚠️ **DİKKAT:** Bu komutlar TÜM VERİLERİ SİLER! Sadece development ortamında kullanın.

---

## 📞 Yardım

Sorun devam ederse:

1. `VERITABANI_HATASI_COZUM.md` dosyasını okuyun
2. Migration dosyalarını kontrol edin
3. phpMyAdmin'de manuel SQL çalıştırın
4. Cache temizleyin: `dotnet clean && dotnet build`

---

## 🎯 Özet

**En hızlı çözüm:**
```sql
-- phpMyAdmin'de çalıştır
ALTER TABLE Konumlar ADD COLUMN EkleyenKullanici VARCHAR(100) NULL;
```

**En güvenli çözüm:**
```bash
dotnet ef database drop --force
dotnet ef database update
```

**Production çözümü:**
```bash
dotnet ef migrations script > migration.sql
# migration.sql dosyasını production'da çalıştır
```
