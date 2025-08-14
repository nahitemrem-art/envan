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
        optionsBuilder.UseNpgsql("Host=dpg-d2e7hoc9c44c73egrho0-a.oregon-postgres.render.com;Port=5432;Database=envanterdb;Username=envanterdb_user;Password=XqwzlDcZklWMQPsb01FBhnTqQllghjxN;SSL Mode=Require");
        return new EnvanterContext(optionsBuilder.Options);
    }
}
