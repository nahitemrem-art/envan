namespace EnvanterTakip.Models
{
    public class Personel
    {
        public int PersonelID { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string BirimAdi { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public bool AktifMi { get; set; }
        public ICollection<Zimmet> Zimmetler { get; set; }
    }
}