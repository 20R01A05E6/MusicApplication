using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Melody.Authorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext
builder.Services.AddDbContext<MelodyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mvcapplication")
    ?? throw new InvalidOperationException("Connection string 'MelodyContext' not found.")));

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseSession(); // Enable session middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


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
