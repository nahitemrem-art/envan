using Microsoft.EntityFrameworkCore;
using EnvanterTakip.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EnvanterContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EnvanterConnection")));

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login"; // Giriş sayfanızın yolu
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Middleware konfigürasyonu...

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Authorization'dan önce ekleyin
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
