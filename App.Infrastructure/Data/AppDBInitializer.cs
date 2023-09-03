using App.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace App.Infrastructure.Data
{
    public static class AppDBInitializer
    {
        public static void SeedSuperAdminUser(UserManager<AppUser> userManager, AppDBContext context)
        {
            if (context.Users.FirstOrDefault(u => u.Email == "admin@app.com") == null)
            {
                var defaultUser = new AppUser
                {
                    UserName = "admin@App.com",
                    Email = "admin@App.com",
                    FirstName = "Admin",
                    LastName = "App",
                    PhoneNumber = "01285473767",
                    EmailConfirmed = true
                };
                userManager.CreateAsync(defaultUser, "Qwe_123456").Wait();
                userManager.AddToRoleAsync(defaultUser, "SuperAdmin").Wait();
                var res = userManager.AddClaimsAsync(defaultUser, new[]
                {
                    new Claim("UserFullName", "Admin App")
                }).Result;
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("SuperAdmin").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "SuperAdmin"
                };
                _ = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Employer").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Employer"
                };
                _ = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Applicant").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Applicant"
                };
                _ = roleManager.CreateAsync(role).Result;
            }

        }


    }
}