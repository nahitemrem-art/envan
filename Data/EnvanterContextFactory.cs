// Data/EnvanterContextFactory.cs
using EnvanterTakip.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class EnvanterContextFactory : IDesignTimeDbContextFactory<EnvanterContext>
{
    public EnvanterContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EnvanterContext>();
        
        // PostgreSQL URL'ini parse et
        var databaseUrl = "postgresql://envanterdb_user:XqwzlDcZklWMQPsb01FBhnTqQllghjxN@dpg-d2e7hoc9c44c73egrho0-a/envanterdb";
        
        string connectionString;
        if (!string.IsNullOrEmpty(databaseUrl) && databaseUrl.StartsWith("postgresql://"))
        {
            // PostgreSQL URL'ini Npgsql formatına çevir
            var uri = new Uri(databaseUrl);
            var userInfo = uri.UserInfo.Split(':');
            connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.Trim('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
        }
        else
        {
            connectionString = databaseUrl;
        }
        
        optionsBuilder.UseNpgsql(connectionString);
        return new EnvanterContext(optionsBuilder.Options);
    }
}
