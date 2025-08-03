using AutoMapper;
using EInvoice.App.Models;
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

builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, AppClaimsPrincipalFactory>();
// configure Identity cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Users/Login";          // redirect when not logged in
    options.AccessDeniedPath = "/Users/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // cookie lifetime
    // Handle AJAX requests properly
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api") &&
                context.Response.StatusCode == 200)
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api") &&
                context.Response.StatusCode == 200)
            {
                context.Response.StatusCode = 403;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        }
    };
});
// Remove JWT and hybrid authentication - just use this:
builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

#endregion
builder.Services.AddControllersWithViews();
var app = builder.Build();

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