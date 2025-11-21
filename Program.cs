using Microsoft.EntityFrameworkCore;
using EnvanterTakip.Data;

var builder = WebApplication.CreateBuilder(args);

// Connection string'i parse et
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

string connectionString;
if (!string.IsNullOrEmpty(databaseUrl) && databaseUrl.StartsWith("mysql://"))
{
    // MySQL URL'ini connection string formatına çevir
    var uri = new Uri(databaseUrl);
    connectionString = $"Server={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.Trim('/')};User={uri.UserInfo.Split(':')[0]};Password={uri.UserInfo.Split(':')[1]};";
}
else
{
    connectionString = databaseUrl ?? builder.Configuration.GetConnectionString("DefaultConnection");
}

var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
builder.Services.AddDbContext<EnvanterContext>(options =>
    options.UseMySql(connectionString, serverVersion));

// Authentication ekle
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EnvanterContext>();
    try
    {
        context.Database.Migrate();
        await DbSeeder.SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı migration veya seed işlemi sırasında hata oluştu");
    }
}

app.Run();
