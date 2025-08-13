using Microsoft.EntityFrameworkCore;
using EnvanterTakip.Models; // Model sınıflarınızın namespace'i

namespace EnvanterTakip.Data
{
    public class EnvanterContext : DbContext
    {
        public EnvanterContext(DbContextOptions<EnvanterContext> options)
            : base(options)
        {
        }

        public DbSet<Cihaz> Cihazlar { get; set; }
        public DbSet<Zimmet> Zimmetler { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Personel> Personeller { get; set; }
    }
}