namespace EnvanterTakip.Models
{
    public class AppUser
    {
        public int AppUserID { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Rol { get; set; } // "Admin", "User" vb.
        public bool AktifMi { get; set; }
    }
}