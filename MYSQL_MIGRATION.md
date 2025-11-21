# PostgreSQL'den MySQL'e Geçiş

Bu doküman, projenin PostgreSQL'den MySQL'e geçişini açıklar.

## Yapılan Değişiklikler

### 1. NuGet Paketleri

**Kaldırıldı:**
```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.4" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.8" />
```

**Eklendi:**
```xml
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.2" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.11" />
```

**Not:** Pomelo.EntityFrameworkCore.MySql EF Core 8.x ile uyumlu olduğu için Design paketinin versiyonunu 8.0.11'e düşürdük.

### 2. Program.cs Değişiklikleri

**Eski (PostgreSQL):**
```csharp
builder.Services.AddDbContext<EnvanterContext>(options =>
    options.UseNpgsql(connectionString));
```

**Yeni (MySQL):**
```csharp
var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
builder.Services.AddDbContext<EnvanterContext>(options =>
    options.UseMySql(connectionString, serverVersion));
```

### 3. Connection String Formatı

**PostgreSQL:**
```
Host=localhost;Port=5432;Database=envanterdb;Username=postgres;Password=postgres
```

**MySQL:**
```
Server=localhost;Port=3306;Database=envanterdb;User=root;Password=root;
```

### 4. EnvanterContextFactory.cs

Factory class MySQL için yeniden yazıldı.

### 5. Migrations

Tüm PostgreSQL migration'ları silindi ve MySQL için yeni migration oluşturuldu:

```bash
# Eski migration'ları sil
rm -rf Migrations/*.cs Migrations/*.Designer.cs

# Yeni migration oluştur
dotnet ef migrations add InitialMySqlMigration
```

## Docker Kurulum

### PostgreSQL Container'ı Durdur

```bash
docker stop postgres-dev
docker rm postgres-dev
```

### MySQL Container'ı Başlat

```bash
docker run -d \
  --name mysql-dev \
  -e MYSQL_ROOT_PASSWORD=root \
  -e MYSQL_DATABASE=envanterdb \
  -p 3306:3306 \
  mysql:8.0
```

## Çalıştırma

```bash
# Paketleri yükle
dotnet restore

# Uygulamayı çalıştır (migration otomatik uygulanır)
dotnet run
```

## Veri Kontrolü

### MySQL CLI ile Bağlan

```bash
docker exec -it mysql-dev mysql -uroot -proot envanterdb
```

### Tabloları Listele

```sql
SHOW TABLES;
```

### Konum Sayısını Kontrol Et

```sql
SELECT COUNT(*) FROM Konumlar;
```

### Kategori Sayısını Kontrol Et

```sql
SELECT COUNT(*) FROM Kategoriler;
```

## Farklar ve Dikkat Edilmesi Gerekenler

### 1. Auto Increment

**PostgreSQL:**
- SERIAL kullanır
- `IDENTITY BY DEFAULT`

**MySQL:**
- AUTO_INCREMENT kullanır

### 2. Boolean Tipler

**PostgreSQL:**
- Native BOOLEAN tipi vardır

**MySQL:**
- TINYINT(1) olarak saklanır
- 0 = false, 1 = true

### 3. Timestamp

**PostgreSQL:**
- `timestamp with time zone`

**MySQL:**
- `timestamp` veya `datetime`
- Timezone bilgisi dahil değildir

### 4. String Comparison

**PostgreSQL:**
- Case-sensitive by default

**MySQL:**
- Case-insensitive by default (collation'a bağlı)

### 5. Limit/Offset

**PostgreSQL:**
```sql
SELECT * FROM table LIMIT 10 OFFSET 20;
```

**MySQL:**
```sql
SELECT * FROM table LIMIT 20, 10;
-- veya
SELECT * FROM table LIMIT 10 OFFSET 20;
```

## Migration Dosyası İçeriği

Yeni migration'da oluşturulan tablolar:

1. **Kategoriler**
   - Id (int, auto-increment)
   - Ad (varchar(100))
   - Aciklama (varchar(500))
   - Ikon (varchar(50))
   - Renk (varchar(20))

2. **Konumlar**
   - Id (int, auto-increment)
   - Ad (varchar(200))
   - Aciklama (varchar(1000))
   - Adres (varchar(500))
   - Telefon (varchar(20))
   - Email (varchar(100))
   - Website (varchar(200))
   - Enlem (double)
   - Boylam (double)
   - KategoriId (int, foreign key)
   - Aktif (tinyint(1))
   - EklenmeTarihi (datetime)
   - ResimUrl (varchar(500))
   - CalismaSaatleri (varchar(500))

3. **Diğer Tablolar** (Mevcut)
   - AppUsers
   - Cihazlar
   - Personeller
   - Zimmetler

## Performans İyileştirmeleri

### Index'ler

MySQL migration otomatik olarak şu index'leri oluşturur:

```sql
CREATE INDEX IX_Konumlar_KategoriId ON Konumlar(KategoriId);
```

### Ek İyileştirmeler (Opsiyonel)

```sql
-- Full-text search için
ALTER TABLE Konumlar ADD FULLTEXT INDEX idx_ft_search (Ad, Aciklama, Adres);

-- Koordinat aramaları için
CREATE INDEX idx_location ON Konumlar(Enlem, Boylam);

-- Aktif konum sorguları için
CREATE INDEX idx_aktif ON Konumlar(Aktif);
```

## Yedekleme

### MySQL Backup

```bash
# Dump oluştur
docker exec mysql-dev mysqldump -uroot -proot envanterdb > backup.sql

# Restore et
docker exec -i mysql-dev mysql -uroot -proot envanterdb < backup.sql
```

## Troubleshooting

### Connection String Hatası

Eğer bağlantı hatası alırsanız, connection string'in doğru olduğundan emin olun:

```
Server=localhost;Port=3306;Database=envanterdb;User=root;Password=root;
```

### Port Kullanımda

Eğer 3306 portu kullanılıyorsa:

```bash
docker run -d --name mysql-dev \
  -e MYSQL_ROOT_PASSWORD=root \
  -e MYSQL_DATABASE=envanterdb \
  -p 3307:3306 \
  mysql:8.0
```

Connection string'i güncelleyin: `Port=3307`

### Character Set Sorunları

Türkçe karakter sorunları için:

```sql
ALTER DATABASE envanterdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

## Sonuç

✅ Proje başarıyla MySQL'e geçirildi  
✅ Tüm tablolar ve ilişkiler korundu  
✅ Seed data MySQL'e yüklendi  
✅ Uygulama test edildi ve çalışıyor

**Test Sonuçları:**
- ✅ 10 kategori oluşturuldu
- ✅ 5 örnek konum eklendi
- ✅ Foreign key ilişkileri çalışıyor
- ✅ Web arayüzü erişilebilir
- ✅ Harita ve listeler görüntüleniyor
