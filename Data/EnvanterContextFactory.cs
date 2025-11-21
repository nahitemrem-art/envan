// Data/EnvanterContextFactory.cs
using EnvanterTakip.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class EnvanterContextFactory : IDesignTimeDbContextFactory<EnvanterContext>
{
    public EnvanterContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EnvanterContext>();
        
        // MySQL connection string (XAMPP - local development)
        var connectionString = "Server=localhost;Port=3306;Database=envanterdb;User=root;Password=;";
        
        // MySQL server version
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
        
        optionsBuilder.UseMySql(connectionString, serverVersion);
        return new EnvanterContext(optionsBuilder.Options);
    }
}
