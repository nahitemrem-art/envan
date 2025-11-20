using EnvanterTakip.Models;
using Microsoft.EntityFrameworkCore;

namespace EnvanterTakip.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(EnvanterContext context)
        {
            if (await context.Kategoriler.AnyAsync())
            {
                return;
            }

            var kategoriler = new List<Kategori>
            {
                new Kategori { Ad = "Hastane", Aciklama = "Hastaneler ve sağlık tesisleri", Ikon = "bi-hospital", Renk = "#dc3545" },
                new Kategori { Ad = "Banka", Aciklama = "Bankalar ve ATM'ler", Ikon = "bi-bank", Renk = "#0d6efd" },
                new Kategori { Ad = "Okul", Aciklama = "Okullar ve eğitim kurumları", Ikon = "bi-mortarboard", Renk = "#198754" },
                new Kategori { Ad = "Park", Aciklama = "Parklar ve yeşil alanlar", Ikon = "bi-tree", Renk = "#20c997" },
                new Kategori { Ad = "Resmi Kurum", Aciklama = "Resmi kurumlar ve devlet daireleri", Ikon = "bi-building", Renk = "#6c757d" },
                new Kategori { Ad = "Alışveriş", Aciklama = "Alışveriş merkezleri ve marketler", Ikon = "bi-shop", Renk = "#fd7e14" },
                new Kategori { Ad = "Restoran", Aciklama = "Restoranlar ve kafeler", Ikon = "bi-cup-hot", Renk = "#ffc107" },
                new Kategori { Ad = "Otopark", Aciklama = "Otopark alanları", Ikon = "bi-p-square", Renk = "#6610f2" },
                new Kategori { Ad = "Kültür Merkezi", Aciklama = "Kültür ve sanat merkezleri", Ikon = "bi-book", Renk = "#d63384" },
                new Kategori { Ad = "Spor Tesisi", Aciklama = "Spor salonları ve tesisleri", Ikon = "bi-trophy", Renk = "#0dcaf0" },
                new Kategori { Ad = "Müze", Aciklama = "Müzeler ve sergi alanları", Ikon = "bi-bank", Renk = "#8b4513" },
                new Kategori { Ad = "Tarihi Mekan", Aciklama = "Tarihi yapılar ve anıtlar", Ikon = "bi-building", Renk = "#8b0000" },
                new Kategori { Ad = "Doğal Alan", Aciklama = "Doğal güzellikler ve mesire alanları", Ikon = "bi-tree", Renk = "#228b22" },
                new Kategori { Ad = "Mesire Alanı", Aciklama = "Piknik ve dinlenme alanları", Ikon = "bi-flower1", Renk = "#32cd32" }
            };

            context.Kategoriler.AddRange(kategoriler);
            await context.SaveChangesAsync();

            var konumlar = new List<Konum>
            {
                new Konum
                {
                    Ad = "Sakarya Üniversitesi Eğitim ve Araştırma Hastanesi",
                    Aciklama = "Tam teşekküllü eğitim ve araştırma hastanesi",
                    Adres = "Korucuk, Sakarya Üniversitesi Kampüsü, 54290 Adapazarı/Sakarya",
                    Telefon = "0264 295 66 66",
                    Enlem = 40.7441,
                    Boylam = 30.3264,
                    KategoriId = kategoriler[0].Id,
                    Aktif = true
                },
                new Konum
                {
                    Ad = "Sakarya Büyükşehir Belediyesi",
                    Aciklama = "Sakarya Büyükşehir Belediye Binası",
                    Adres = "Merkez, Tavşanlı Mahallesi, 54050 Adapazarı/Sakarya",
                    Telefon = "0264 275 40 00",
                    Website = "https://www.sakarya.bel.tr",
                    Enlem = 40.7569,
                    Boylam = 30.4030,
                    KategoriId = kategoriler[4].Id,
                    Aktif = true,
                    CalismaSaatleri = "Pazartesi-Cuma 08:30-17:30"
                },
                new Konum
                {
                    Ad = "Kent Meydanı",
                    Aciklama = "Sakarya'nın merkez meydanı ve toplanma noktası",
                    Adres = "Cumhuriyet Mahallesi, Adapazarı/Sakarya",
                    Enlem = 40.7835,
                    Boylam = 30.4061,
                    KategoriId = kategoriler[3].Id,
                    Aktif = true
                },
                new Konum
                {
                    Ad = "Sakarya Park AVM",
                    Aciklama = "Alışveriş merkezi",
                    Adres = "Yağcılar Mahallesi, 54100 Adapazarı/Sakarya",
                    Telefon = "0264 277 54 54",
                    Enlem = 40.7650,
                    Boylam = 30.3900,
                    KategoriId = kategoriler[5].Id,
                    Aktif = true,
                    CalismaSaatleri = "Her gün 10:00-22:00"
                },
                new Konum
                {
                    Ad = "Adapazarı Müzesi",
                    Aciklama = "Şehir tarihi ve kültür müzesi",
                    Adres = "Orta Mahalle, Adapazarı/Sakarya",
                    Telefon = "0264 274 13 48",
                    Enlem = 40.7812,
                    Boylam = 30.4025,
                    KategoriId = kategoriler[8].Id,
                    Aktif = true,
                    CalismaSaatleri = "Salı-Pazar 09:00-17:00"
                }
            };

            context.Konumlar.AddRange(konumlar);
            await context.SaveChangesAsync();
        }
    }
}
