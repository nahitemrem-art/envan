# Sakarya Kent Rehberi - Yeni Özellikler

Bu doküman, Sakarya Belediyesi Kent Rehberi web sitesine benzer şekilde eklenen yeni özellikleri açıklar.

## Eklenen Özellikler

### 1. İnteraktif Harita Görünümü
- **URL**: `/Harita/Index` veya `/Harita/Kent`
- **Özellikler**:
  - OpenStreetMap tabanlı Leaflet.js harita entegrasyonu
  - Harita üzerinde konumların işaretleyicilerle gösterimi
  - Tıklanabilir popup'lar ile konum bilgileri
  - Responsive tasarım - mobil uyumlu

### 2. Kategori Sistemi
- **Model**: `Models/Kategori.cs`
- **Controller**: `Controllers/KategoriController.cs`
- **Özellikler**:
  - Kategori adı, açıklama, ikon, renk
  - Bootstrap Icons desteği
  - Renkli badge'ler ile görsel ayırt
  - CRUD işlemleri (Create, Read, Update, Delete)

**Örnek Kategoriler**:
- Hastane (kırmızı, bi-hospital)
- Banka (mavi, bi-bank)
- Okul (yeşil, bi-mortarboard)
- Park (turkuaz, bi-tree)
- Resmi Kurum (gri, bi-building)
- Alışveriş (turuncu, bi-shop)
- Restoran (sarı, bi-cup-hot)
- Otopark (mor, bi-p-square)
- Kültür Merkezi (pembe, bi-book)
- Spor Tesisi (açık mavi, bi-trophy)

### 3. Konum Yönetimi
- **Model**: `Models/Konum.cs`
- **Controller**: `Controllers/KonumController.cs`
- **Özellikler**:
  - Ad, açıklama, adres, telefon, email, website
  - GPS koordinatları (enlem/boylam)
  - Kategori ilişkisi
  - Aktif/Pasif durumu
  - Ekleme tarihi
  - Resim URL'i
  - Çalışma saatleri
  - Arama ve filtreleme

### 4. Kullanıcı Arayüzü

#### Ana Sayfa (`/Home/Index`)
- Karşılama mesajı
- Haritayı görüntüleme butonu
- Özellik kartları (harita, arama, detaylı bilgiler)
- Bootstrap Icons ile modern tasarım

#### Harita Görünümü (`/Harita/Index`)
- Sol panel: Konum listesi
  - Kategori badge'leri
  - Adres ve telefon bilgileri
  - Tıklanabilir liste öğeleri
- Sağ panel: İnteraktif harita
  - Tüm konumların işaretleyicileri
  - Popup detaylar
  - Harita kontrolleri
- Üst panel: Arama ve filtreleme
  - Metin arama
  - Kategori filtreleme
  - Temizle butonu

#### Konum Detay Sayfası (`/Harita/Detay/{id}`)
- Tam konum bilgileri
- Resim gösterimi (varsa)
- İletişim bilgileri (telefon, email, website)
- Çalışma saatleri
- Konum için küçük harita
- Breadcrumb navigasyon

### 5. Admin Yönetim Paneli

#### Kategori Yönetimi (`/Kategori/*`)
- **Index**: Tüm kategorileri listele
- **Create**: Yeni kategori ekle
  - Renk seçici (color picker)
  - İkon girişi (Bootstrap Icons)
- **Edit**: Kategori düzenle
- **Delete**: Kategori sil (onay ile)

#### Konum Yönetimi (`/Konum/*`)
- **Index**: Tüm konumları listele
  - Kategori filtreleme
  - Arama
  - Aktif/Pasif durumu gösterimi
- **Create**: Yeni konum ekle
  - Tüm detayları giriş formu
  - Koordinat alma yardımı
  - Kategori seçimi
- **Edit**: Konum düzenle
- **Delete**: Konum sil (onay ile)

### 6. Veritabanı Yapısı

#### Kategoriler Tablosu
```sql
CREATE TABLE Kategoriler (
    Id SERIAL PRIMARY KEY,
    Ad VARCHAR(100) NOT NULL,
    Aciklama VARCHAR(500),
    Ikon VARCHAR(50),
    Renk VARCHAR(20)
);
```

#### Konumlar Tablosu
```sql
CREATE TABLE Konumlar (
    Id SERIAL PRIMARY KEY,
    Ad VARCHAR(200) NOT NULL,
    Aciklama VARCHAR(1000),
    Adres VARCHAR(500),
    Telefon VARCHAR(20),
    Email VARCHAR(100),
    Website VARCHAR(200),
    Enlem DOUBLE PRECISION NOT NULL,
    Boylam DOUBLE PRECISION NOT NULL,
    KategoriId INTEGER NOT NULL REFERENCES Kategoriler(Id),
    Aktif BOOLEAN NOT NULL DEFAULT TRUE,
    EklenmeTarihi TIMESTAMP NOT NULL,
    ResimUrl VARCHAR(500),
    CalismaSaatleri VARCHAR(500)
);
```

### 7. Örnek Veri (Data Seeding)
- 10 kategori otomatik oluşturulur
- 5 örnek konum eklenir:
  - Sakarya Üniversitesi Eğitim ve Araştırma Hastanesi
  - Sakarya Büyükşehir Belediyesi
  - Kent Meydanı
  - Sakarya Park AVM
  - Adapazarı Müzesi

### 8. Güvenlik ve Erişim Kontrolü
- Harita görünümü: Herkese açık (authentication gerektirmez)
- Konum/Kategori yönetimi: Admin girişi gerektirir (`[Authorize]` attribute)
- Navigation menüsü kullanıcı durumuna göre dinamik

### 9. Responsive Tasarım
- Bootstrap 5 grid system
- Mobil uyumlu liste ve harita görünümü
- Responsive tablo tasarımları
- Touch-friendly arayüz

### 10. API Endpoint
- **GET** `/Harita/GetKonumlar?kategoriId={id}`: JSON formatında konum listesi
  - AJAX çağrıları için hazır
  - Kategori filtreleme desteği

## Teknoloji Stack

### Frontend
- **Bootstrap 5**: UI framework
- **Bootstrap Icons**: Icon seti
- **Leaflet.js 1.9.4**: Açık kaynak harita kütüphanesi
- **OpenStreetMap**: Harita tile sağlayıcı
- **Vanilla JavaScript**: Harita etkileşimleri

### Backend
- **ASP.NET Core 8 MVC**: Web framework
- **Entity Framework Core**: ORM
- **Npgsql**: PostgreSQL sürücüsü
- **Razor Views**: Template engine

## Kullanım Senaryoları

### Senaryo 1: Ziyaretçi Haritayı Görüntülüyor
1. Ana sayfaya giriş yapılır
2. "Haritayı Görüntüle" butonuna tıklanır
3. Harita ve konum listesi gösterilir
4. Kategori seçilerek filtreleme yapılır
5. Listeden bir konum seçilir, harita o konuma odaklanır
6. İşaretleyiciye tıklanarak detaylar görülür

### Senaryo 2: Admin Yeni Konum Ekliyor
1. Admin olarak giriş yapılır
2. "Konumlar" menüsüne tıklanır
3. "Yeni Konum Ekle" butonuna basılır
4. Form doldurulur:
   - Google Maps'ten koordinatlar alınır
   - Kategori seçilir
   - İletişim bilgileri girilir
5. "Kaydet" butonuna basılır
6. Konum haritada görünür hale gelir

### Senaryo 3: Kullanıcı Hastane Arıyor
1. Harita sayfası açılır
2. Kategori dropdown'dan "Hastane" seçilir
3. Liste otomatik filtrelenir
4. Haritada sadece hastaneler (kırmızı işaretleyiciler) gösterilir
5. Bir hastaneye tıklanır
6. "Detaylar" butonuna basılır
7. Hastane bilgileri ve haritası gösterilir

## Gelecek Geliştirmeler İçin Öneriler

1. **Gelişmiş Arama**: Mesafe bazlı arama, "bana en yakın" özelliği
2. **Rota Hesaplama**: İki nokta arası yol tarifi
3. **Kullanıcı Yorumları**: Konumlar için yorum ve puanlama sistemi
4. **Resim Yükleme**: Admin panelinden resim upload özelliği
5. **Excel/CSV İçe Aktarma**: Toplu konum ekleme
6. **Çoklu Dil Desteği**: Türkçe/İngilizce seçeneği
7. **Mobil Uygulama**: React Native veya Flutter ile mobil app
8. **Bildirimler**: Yeni konum eklendiğinde bildirim
9. **Favori Konumlar**: Kullanıcıların favorilere eklemesi
10. **Sosyal Medya Paylaşımı**: Konum paylaşım butonları

## Referans

Bu proje https://rehber.sakarya.bel.tr/harita/kent sitesinden esinlenilerek geliştirilmiştir.
