# Sorun Giderme Rehberi

## 🔴 Yaygın Hatalar ve Çözümleri

### 1. "Unable to connect to any of the specified MySQL hosts"

**Neden:** MySQL container çalışmıyor veya hazır değil.

**Çözüm 1:** Container'ı kontrol edin
```bash
docker ps | grep mysql
```

Eğer çıktı yoksa, başlatın:
```bash
docker run -d --name mysql-dev -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=envanterdb -p 3306:3306 mysql:8.0
sleep 20  # MySQL'in hazır olmasını bekleyin
```

**Çözüm 2:** Container durmuşsa, başlatın
```bash
docker start mysql-dev
sleep 10
```

**Çözüm 3:** Container bozuksa, yeniden oluşturun
```bash
docker stop mysql-dev
docker rm mysql-dev
docker run -d --name mysql-dev -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=envanterdb -p 3306:3306 mysql:8.0
sleep 20
```

### 2. "Port 3306 already in use"

**Neden:** Başka bir MySQL veya uygulama 3306 portunu kullanıyor.

**Çözüm 1:** Mevcut MySQL'i durdurun
```bash
# Eğer sistem MySQL'i varsa
sudo systemctl stop mysql

# Veya başka bir Docker container
docker ps | grep 3306
docker stop <container-id>
```

**Çözüm 2:** Farklı port kullanın
```bash
docker run -d --name mysql-dev -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=envanterdb -p 3307:3306 mysql:8.0
```

`appsettings.json` dosyasını düzenleyin:
```json
"DefaultConnection": "Server=localhost;Port=3307;Database=envanterdb;User=root;Password=root;"
```

### 3. "Migration failed" veya Veritabanı Hataları

**Çözüm 1:** Veritabanını sıfırlayın
```bash
docker exec -it mysql-dev mysql -uroot -proot -e "DROP DATABASE IF EXISTS envanterdb; CREATE DATABASE envanterdb;"
dotnet run
```

**Çözüm 2:** Migration'ları manuel uygulayın
```bash
dotnet ef database drop --force
dotnet ef database update
```

**Çözüm 3:** Tüm container'ı yeniden oluşturun
```bash
docker stop mysql-dev
docker rm mysql-dev
docker volume prune -f  # DİKKAT: Tüm Docker volume'leri siler!
docker run -d --name mysql-dev -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=envanterdb -p 3306:3306 mysql:8.0
sleep 20
dotnet run
```

### 4. Build Hataları

**Hata:** "The type or namespace name could not be found"

**Çözüm:**
```bash
dotnet restore
dotnet clean
dotnet build
```

**Hata:** "Package version conflict"

**Çözüm:** NuGet cache'i temizleyin
```bash
dotnet nuget locals all --clear
dotnet restore
dotnet build
```

### 5. Uygulama Başlamıyor

**Kontrol 1:** .NET SDK versiyonu
```bash
dotnet --version
# Çıktı: 8.0.x olmalı
```

Eğer değilse, .NET 8 SDK'yı yükleyin.

**Kontrol 2:** Port kullanımda mı?
```bash
netstat -an | grep 5157
# veya
lsof -i :5157
```

Eğer kullanımdaysa, uygulamayı durdurun:
```bash
pkill -f "dotnet run"
```

**Kontrol 3:** Dosya izinleri
```bash
chmod +x start.sh
chmod +x test-app.sh
```

### 6. "No data seeded" - Veriler Yüklenmiyor

**Kontrol:** Seed işlemi çalıştı mı?
```bash
docker exec mysql-dev mysql -uroot -proot -e "USE envanterdb; SELECT COUNT(*) FROM Kategoriler;"
```

Eğer 0 ise, seed işlemini manuel çalıştırın:
```bash
# Program.cs'deki seed kodu zaten var, sadece yeniden başlatın
dotnet run
```

Veya manuel SQL:
```sql
docker exec -it mysql-dev mysql -uroot -proot envanterdb

-- Örnek kategori ekle
INSERT INTO Kategoriler (Ad, Aciklama, Ikon, Renk)
VALUES ('Hastane', 'Hastaneler', 'bi-hospital', '#dc3545');
```

### 7. Leaflet Haritası Görünmüyor

**Kontrol 1:** CDN bağlantısı
```bash
curl -I https://unpkg.com/leaflet@1.9.4/dist/leaflet.css
# HTTP/2 200 olmalı
```

**Kontrol 2:** JavaScript hataları
- Tarayıcıda F12 basın
- Console sekmesini kontrol edin

**Kontrol 3:** Koordinatlar doğru mu?
```sql
docker exec mysql-dev mysql -uroot -proot -e "USE envanterdb; SELECT Ad, Enlem, Boylam FROM Konumlar LIMIT 5;"
```

### 8. Admin Sayfalarına Erişilemiyor

**Neden:** Giriş yapılmamış veya yetki yok.

**Çözüm:** Admin kullanıcısı oluşturun
```sql
docker exec -it mysql-dev mysql -uroot -proot envanterdb

INSERT INTO AppUsers (KullaniciAdi, Sifre, Rol, AktifMi)
VALUES ('admin', 'admin123', 'Admin', true);
```

Sonra `/Account/Login` sayfasından giriş yapın.

### 9. Docker Komutları Çalışmıyor

**Hata:** "Cannot connect to Docker daemon"

**Çözüm:**
```bash
# Docker servisini başlat
sudo systemctl start docker

# Kullanıcıyı docker grubuna ekle
sudo usermod -aG docker $USER
# Oturumu yeniden açın
```

### 10. Türkçe Karakterler Bozuk Görünüyor

**Çözüm:** MySQL karakter setini kontrol edin
```sql
docker exec mysql-dev mysql -uroot -proot -e "SHOW VARIABLES LIKE 'character_set%';"
```

Düzeltme:
```sql
docker exec mysql-dev mysql -uroot -proot -e "ALTER DATABASE envanterdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;"
```

## 🛠️ Diagnostic Komutlar

### Sistem Kontrolü
```bash
# .NET versiyonu
dotnet --version

# Docker versiyonu
docker --version

# MySQL container durumu
docker ps -a | grep mysql

# MySQL logları
docker logs mysql-dev | tail -50

# Uygulama build
dotnet build --no-restore

# Port kullanımı
netstat -tlnp | grep -E "3306|5157"
```

### Veritabanı Kontrolü
```bash
# MySQL'e bağlan
docker exec -it mysql-dev mysql -uroot -proot envanterdb

# Tabloları listele
SHOW TABLES;

# Her tablodaki kayıt sayısı
SELECT 'Kategoriler' as Tablo, COUNT(*) as Kayit FROM Kategoriler
UNION ALL
SELECT 'Konumlar', COUNT(*) FROM Konumlar
UNION ALL
SELECT 'AppUsers', COUNT(*) FROM AppUsers;

# Son eklenen konumlar
SELECT Ad, Kategori.Ad as Kategori, EklenmeTarihi 
FROM Konumlar 
JOIN Kategoriler as Kategori ON Konumlar.KategoriId = Kategori.Id 
ORDER BY EklenmeTarihi DESC 
LIMIT 5;
```

### Uygulama Kontrolü
```bash
# Uygulamanın çalışıp çalışmadığını test et
curl -I http://localhost:5157/

# Ana sayfa HTML kontrolü
curl http://localhost:5157/ | head -30

# Harita sayfası kontrolü
curl http://localhost:5157/Harita | grep -i "sakarya"

# API endpoint test
curl http://localhost:5157/Harita/GetKonumlar | jq .
```

## 📞 Yardım Alma

Eğer sorun devam ediyorsa:

1. **Logları toplayın:**
```bash
# Uygulama logları
dotnet run > app.log 2>&1 &
tail -f app.log

# MySQL logları
docker logs mysql-dev > mysql.log 2>&1
```

2. **Sistem bilgilerini toplayın:**
```bash
echo "OS: $(uname -a)"
echo ".NET: $(dotnet --version)"
echo "Docker: $(docker --version)"
docker ps -a | grep mysql
```

3. **Test script'i çalıştırın:**
```bash
./test-app.sh
```

## 🔄 Tam Yeniden Başlatma

Eğer hiçbir şey işe yaramazsa, her şeyi sıfırdan başlatın:

```bash
# 1. Uygulamayı durdur
pkill -f "dotnet"

# 2. MySQL'i temizle
docker stop mysql-dev
docker rm mysql-dev
docker volume rm $(docker volume ls -q | grep mysql) 2>/dev/null

# 3. Build temizle
cd /home/engine/project
dotnet clean
rm -rf bin/ obj/

# 4. Yeniden başlat
dotnet restore
docker run -d --name mysql-dev -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=envanterdb -p 3306:3306 mysql:8.0
sleep 20
dotnet run
```

---

**Not:** Bu rehber en yaygın sorunları kapsar. Başka bir sorun yaşıyorsanız, lütfen hata mesajını tam olarak kaydedin ve GitHub issues'da bildirin.
