﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment1.Models;

namespace Assignment1.Data
{
    public static class DbInitializer
    {
        public static string adminPassword;

        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            // create the database if it doesn't exist
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if roles already exist and exit if there are
            if (roleManager.Roles.Count() > 0)
                return 1;  // should log an error message here

            // Seed roles
            int result = await SeedRoles(roleManager);
            if (result != 0)
                return 2;  // should log an error message here

            // Check if users already exist and exit if there are
            if (userManager.Users.Count() > 0)
                return 3;  // should log an error message here

            // Seed users
            result = await SeedUsers(userManager);
            if (result != 0)
                return 4;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Create Admin Role
            var result = await roleManager.CreateAsync(new IdentityRole("Manager"));
            if (!result.Succeeded)
                return 1;  // should log an error message here

            // Create Member Role
            result = await roleManager.CreateAsync(new IdentityRole("Employee"));
            if (!result.Succeeded)
                return 2;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Create Admin User
            var adminUser = new ApplicationUser
            {
                UserName = "the.admin@mohawkcollege.ca",
                Email = "the.admin@mohawkcollege.ca",
                FirstName = "The",
                LastName = "Admin",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
                return 1;  // should log an error message here

            // Assign user to Admin role
            result = await userManager.AddToRoleAsync(adminUser, "Manager");
            if (!result.Succeeded)
                return 2;  // should log an error message here

            // Create Member User
            var memberUser = new ApplicationUser
            {
                UserName = "the.member@mohawkcollege.ca",
                Email = "the.member@mohawkcollege.ca",
                FirstName = "The",
                LastName = "Member",
                EmailConfirmed = true
            };
            result = await userManager.CreateAsync(memberUser, adminPassword);
            if (!result.Succeeded)
                return 3;  // should log an error message here

            // Assign user to Member role
            result = await userManager.AddToRoleAsync(memberUser, "Employee");
            if (!result.Succeeded)
                return 4;  // should log an error message here

            return 0;
        }
    }
}
