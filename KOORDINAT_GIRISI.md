# 📍 Koordinat Girişi Rehberi

## ✅ Sorun Çözüldü

Koordinat girişindeki "Please enter a multiple of 1e-7" hatası düzeltildi.

### Yapılan Değişiklikler

**Views/Konum/Create.cshtml** ve **Views/Konum/Edit.cshtml** dosyalarında:

```html
<!-- Eski -->
<input asp-for="Enlem" type="number" step="0.0000001" class="form-control" />

<!-- Yeni -->
<input asp-for="Enlem" type="number" step="any" class="form-control" placeholder="40.777648" />
```

`step="any"` ile herhangi bir ondalık sayı kabul edilir hale geldi.

## 🎯 Doğru Koordinat Girişi

### ✔️ Doğru Format

**40.777648** ve **30.403113** ✅

- Nokta (.) kullanın, virgül (,) DEĞİL
- Türkçe klavyede nokta için: **Shift + , (virgül tuşu)**
- Veya sayısal tuş takımındaki nokta

### ❌ Yanlış Format

**40,777648** ve **30,403113** ❌ (virgül kullanılmış)

## 📱 Google Maps'ten Koordinat Alma

### Yöntem 1: Sağ Tıklama

1. Google Maps'i açın: https://maps.google.com
2. Haritada istediğiniz yere **sağ tıklayın**
3. Açılan menüden **"Burası ne?"** (What's here?) seçeneğine tıklayın
4. Alt kısımda koordinatlar görünecek
5. Koordinatlara tıklayın
6. Sol panelde tam koordinatlar görünecek: `40.777648, 30.403113`
7. Kopyalayın ve uygulamaya yapıştırın

### Yöntem 2: URL'den

1. Google Maps'te konumu bulun
2. Adres çubuğundaki URL'ye bakın
3. URL'de `@40.777648,30.403113` gibi koordinatlar vardır
4. Bu değerleri kopyalayın

### Yöntem 3: Arama Çubuğu

1. Google Maps arama çubuğuna koordinatları yazın
2. Format: `40.777648, 30.403113`
3. Enter'a basın
4. Konum işaretlenecek

## 💡 İpuçları

### Sakarya İçin Koordinat Aralıkları

- **Enlem (Latitude):** 40.5 - 41.0
- **Boylam (Longitude):** 30.0 - 31.0

### Örnek Sakarya Konumları

| Konum | Enlem | Boylam |
|-------|-------|--------|
| Adapazarı Merkez | 40.7836 | 30.4067 |
| Sakarya Üniversitesi | 40.7567 | 30.3780 |
| Kent Meydanı | 40.7769 | 30.4018 |
| Atatürk Stadyumu | 40.7588 | 30.3897 |

### Koordinat Hassasiyeti

- **4 ondalık basamak:** ~11 metre hassasiyet
- **5 ondalık basamak:** ~1.1 metre hassasiyet
- **6 ondalık basamak:** ~11 cm hassasiyet

Şehir içi konumlar için **6 ondalık basamak** yeterlidir:
- Örn: `40.777648, 30.403113`

## 🔧 Sorun Giderme

### Hala "Please enter a multiple of" Hatası

**Neden:** Tarayıcı önbelleği

**Çözüm:**
1. Sayfayı yenileyin: **Ctrl + F5** (Windows) veya **Cmd + Shift + R** (Mac)
2. Tarayıcı önbelleğini temizleyin
3. Tarayıcıyı kapatıp yeniden açın

### Virgül (,) Yerine Nokta (.)

Türkçe klavyede:
- **Shift + ,** (virgül tuşu) = nokta
- Veya NumPad'deki **. (nokta)** tuşunu kullanın

### Kopyala-Yapıştır Sorunları

Google Maps'ten kopyalarken bazen format bozulabilir:

**Kopyalanan:** `40.777648, 30.403113` (virgül ve boşluk ile)

**Yapmanız gereken:**
1. İlk sayıyı Enlem kutusuna yapıştırın: `40.777648`
2. İkinci sayıyı Boylam kutusuna yapıştırın: `30.403113`

## 📝 Örnek Kullanım

### Yeni Konum Ekleme

1. **Konum Yönetimi** > **Yeni Konum Ekle**'ye gidin
2. **Ad:** Sakarya Park AVM
3. **Kategori:** Alışveriş
4. **Enlem:** `40.777648` (nokta ile)
5. **Boylam:** `30.403113` (nokta ile)
6. Diğer bilgileri doldurun
7. **Kaydet** butonuna tıklayın

### Mevcut Konum Düzenleme

1. **Konum Yönetimi** listesinden düzenlemek istediğiniz konumu seçin
2. **Düzenle** butonuna tıklayın
3. Koordinatlar kutularda görünecek
4. Değiştirmek isterseniz, yeni değerleri **nokta** ile girin
5. **Kaydet**

## 🌐 Alternatif Koordinat Kaynakları

### OpenStreetMap

1. https://www.openstreetmap.org adresine gidin
2. Haritada konuma sağ tıklayın
3. "Show address" seçeneğini tıklayın
4. Sol panelde koordinatlar görünecek

### Yandex Maps

1. https://yandex.com.tr/harita adresine gidin
2. Konuma sağ tıklayın
3. "Burası ne?" seçeneğine tıklayın
4. Koordinatlar görünecek

### Bing Maps

1. https://www.bing.com/maps adresine gidin
2. Konuma sağ tıklayın
3. "Directions from here" veya "Directions to here" seçin
4. Adres çubuğunda koordinatlar görünecek

## 📱 Mobil Cihazlardan

### Google Maps (Mobil)

1. Google Maps uygulamasını açın
2. Haritada bir noktaya basılı tutun
3. Kırmızı pin düşecek
4. Alt kısımda bilgi kartı açılacak
5. Koordinatlar görünecek
6. Koordinatlara dokunarak kopyalayın

### iOS Haritalar

1. Haritalar uygulamasını açın
2. Konuma basılı tutun
3. Pin düşecek
4. Pin'e dokunun
5. "Koordinatlar" bilgisini görüntüleyin

## ✅ Kontrol Listesi

Konum eklerken/düzenlerken:

- [ ] Koordinatları Google Maps'ten aldım
- [ ] Nokta (.) kullandım, virgül (,) değil
- [ ] Enlem 40.5-41.0 arasında (Sakarya için)
- [ ] Boylam 30.0-31.0 arasında (Sakarya için)
- [ ] 6 ondalık basamak kullandım
- [ ] Haritada doğru yerde görünüyor mu kontrol ettim

---

**Artık sorunsuz konum ekleyebilirsiniz! 🎉**

Hala sorun yaşıyorsanız:
- Tarayıcı önbelleğini temizleyin (Ctrl + F5)
- [TROUBLESHOOTING.md](TROUBLESHOOTING.md) dosyasına bakın
