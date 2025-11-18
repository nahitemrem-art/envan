using System.ComponentModel.DataAnnotations;

namespace EnvanterTakip.Models
{
    public class Konum
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Konum adı zorunludur")]
        [StringLength(200)]
        public string Ad { get; set; }

        [StringLength(1000)]
        public string? Aciklama { get; set; }

        [StringLength(500)]
        public string? Adres { get; set; }

        [StringLength(20)]
        public string? Telefon { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? Website { get; set; }

        [Required(ErrorMessage = "Enlem zorunludur")]
        public double Enlem { get; set; }

        [Required(ErrorMessage = "Boylam zorunludur")]
        public double Boylam { get; set; }

        public int KategoriId { get; set; }
        public Kategori? Kategori { get; set; }

        public bool Aktif { get; set; } = true;

        public DateTime EklenmeTarihi { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? ResimUrl { get; set; }

        [StringLength(500)]
        public string? CalismaSaatleri { get; set; }
    }
}
