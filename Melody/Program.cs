using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MelodyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mvcapplication") ?? throw new InvalidOperationException("Connection string 'MelodyContext' not found.")));

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Enable session middleware
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

/*app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Albums",
        pattern: "{controller=Artists}/{action=Details}/{id?}"
        );

    endpoints.MapControllerRoute(
        name: "Artists",
        pattern: "{controller=Artists}/{action=Details}/{id?}"
        );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );
});*/

app.Run();
