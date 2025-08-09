using EInvoice.Domain.Entities;
using EInvoice.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EInvoice.Service.Helpers
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            // Create roles if they don’t exist
            if (!await roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.SuperAdmin));

            if (!await roleManager.RoleExistsAsync(UserRoles.OrganizationAdmin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.OrganizationAdmin));

            // Create SuperAdmin user if not exists
            var adminEmail = "admin@einvoice.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = "superadmin",
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = UserRoles.OrganizationAdmin,
                    EmailConfirmed = true,
                    IsAdmin = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, UserRoles.SuperAdmin);
                }
            }
        }
        public static async Task SeedRefEntities(IServiceProvider serviceProvider)
        {
            using (var context = new EInvoiceContext(
                serviceProvider.GetRequiredService<DbContextOptions<EInvoiceContext>>()))
            {
                // Check if table already has data
                if (context.InvoiceTypes.Any())
                    return; // Data already exists

                // Add initial data
                context.InvoiceTypes.AddRange(
                    new InvoiceType { Type = "Sales Invoice" },
                    new InvoiceType { Type = "Debit Note" }
                );

                context.SaveChanges();
            }
        }
    }
}
