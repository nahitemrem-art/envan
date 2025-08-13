// Models/Zimmet.cs
using EnvanterTakip.Models; // Bu satırı ekleyin

namespace EnvanterTakip.Models
{
    public class Zimmet
    {
        public int ZimmetID { get; set; }
        public int CihazID { get; set; }
        public int PersonelID { get; set; }
        public DateTime ZimmetTarihi { get; set; }
        public DateTime? IadeTarihi { get; set; }
        public string Aciklama { get; set; }
        
        public Cihaz Cihaz { get; set; } // Artık hata vermemeli
        public Personel Personel { get; set; }
    }
}