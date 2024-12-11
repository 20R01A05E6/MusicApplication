using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext
builder.Services.AddDbContextPool<MelodyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mvcapplication")
    ?? throw new InvalidOperationException("Connection string 'MelodyContext' not found.")));


/*builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Set the sign-in scheme to cookies
})
    .AddCookie()
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration["GoogleKeys:ClientId"];
        options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"];
        options.CallbackPath = "/Signup/signin-google";
    });*/


// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        // Custom event to retrieve token from cookie
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["JwtToken"]; // Get token from cookie
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token; // Token validation
                }
                return Task.CompletedTask;
            }
        };
    });
    //.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));


// Add role-based authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Free", policy => policy.RequireRole("Free"));
    options.AddPolicy("Bronze", policy => policy.RequireRole("Bronze"));
    options.AddPolicy("Silver", policy => policy.RequireRole("Silver"));
    options.AddPolicy("Gold", policy => policy.RequireRole("Gold"));
});

//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Signup/Login"; // Redirect to login page
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
/*builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});*/

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
