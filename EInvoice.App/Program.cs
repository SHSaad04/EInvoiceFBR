using AutoMapper;
using EInvoice.Domain.Entities;
using EInvoice.Infrastructure;
using EInvoice.Service.Aggregates;
using EInvoice.Service.Helpers;
using EInvoice.Service.Implements;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
IConfiguration Configuration = builder.Configuration;
var allowedOrigins = Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
#region Custom Services
// Add services to the container.
builder.Services.AddDbContext<EInvoiceContext>();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<IInvoiceService, InvoiceService>();
builder.Services.AddTransient<IOrganizationService, OrganizationService>();
builder.Services.AddTransient<IUserService, UserService>();
#endregion
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<EInvoiceContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true; // Optional, can be false if not needed.
    options.SignIn.RequireConfirmedEmail = false;  // Disable email confirmation requirement.
    options.SignIn.RequireConfirmedPhoneNumber = false; // Disable phone confirmation.
    options.SignIn.RequireConfirmedPhoneNumber = false; // Disable phone confirmation.
});
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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
