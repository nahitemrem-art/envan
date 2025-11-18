using System.ComponentModel.DataAnnotations;

namespace EnvanterTakip.Models
{
    public class Kategori
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100)]
        public string Ad { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [StringLength(50)]
        public string? Ikon { get; set; }

        [StringLength(20)]
        public string? Renk { get; set; }

        public ICollection<Konum>? Konumlar { get; set; }
    }
}
