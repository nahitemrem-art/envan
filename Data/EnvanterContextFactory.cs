// Data/EnvanterContextFactory.cs
using EnvanterTakip.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class EnvanterContextFactory : IDesignTimeDbContextFactory<EnvanterContext>
{
    public EnvanterContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EnvanterContext>();
        optionsBuilder.UseSqlServer("Server=DESKTOP-RH7A4R0\\SQLEXPRESS;Database=EnvanterTakip;User Id=sa;Password=Admin123;TrustServerCertificate=True;");
        return new EnvanterContext(optionsBuilder.Options);
    }
}