namespace EnvanterTakip.Models
{
    public class Cihaz
    {
        public int CihazID { get; set; }
        public string CihazTuru { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string SeriNo { get; set; }
        public DateTime AlimTarihi { get; set; }
        public DateTime? GarantiBitisTarihi { get; set; }
        public string Aciklama { get; set; }
        public string Durum { get; set; }
        
        public ICollection<Zimmet> Zimmetler { get; set; }

        // Yeni eklenen property
        public string MarkaModelSeriNo => $"{Marka} {Model} (SN:{SeriNo})";
    }
}