using AutoMapper;
using EInvoice.Domain.Entities;
using EInvoice.Infrastructure;
using EInvoice.Service.Aggregates;
using EInvoice.Service.Helpers;
using EInvoice.Service.Implements;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IConfiguration Configuration = builder.Configuration;
var allowedOrigins = Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
#region CUSTOM SERVICES
// Add services to the container.
builder.Services.AddDbContext<EInvoiceContext>();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<IInvoiceService, InvoiceService>();
builder.Services.AddTransient<IOrganizationService, OrganizationService>();
builder.Services.AddTransient<IUserService, UserService>();
#endregion
#region IDENTITY CONFIGURATION
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<EInvoiceContext>()
.AddDefaultTokenProviders();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>,
    UserClaimsPrincipalFactory<User, IdentityRole>>();
// configure Identity cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Users/Login";          // redirect when not logged in
    options.AccessDeniedPath = "/Users/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // cookie lifetime
});

// JWT config for AJAX calls
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) // ✅ Add Cookie Auth
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

#endregion
builder.Services.AddControllersWithViews();
var app = builder.Build();
#region IDENTITY ROLES CREATION
using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedRolesAndAdmin(scope.ServiceProvider);
}
#endregion
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.Run();