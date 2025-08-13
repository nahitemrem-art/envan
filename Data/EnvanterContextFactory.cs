// Data/EnvanterContextFactory.cs
using EnvanterTakip.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL;

public class EnvanterContextFactory : IDesignTimeDbContextFactory<EnvanterContext>
{
    public EnvanterContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EnvanterContext>();
        optionsBuilder.UseNpgsql("postgresql://envanterdb_user:XqwzlDcZklWMQPsb01FBhnTqQllghjxN@dpg-d2e7hoc9c44c73egrho0-a/envanterdb");
        return new EnvanterContext(optionsBuilder.Options);
    }
}
