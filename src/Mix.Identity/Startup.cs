// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.


using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Identity.Data;
using Mix.Identity.Models;

namespace Mix.Identity
{
    public static class Startup
    {
        public static void ConfigIdentity(
            IServiceCollection services, IConfigurationRoot Configuration, string connectionName)
        {
            string connectionString = Configuration.GetConnectionString(connectionName);
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-Mix.Cms.Db;Trusted_Connection=True;MultipleActiveResultSets=true";
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            PasswordOptions pOpt = new PasswordOptions()
            {
                RequireDigit = false,
                RequiredLength = 6,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequireUppercase = false
            };

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password = pOpt;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<ApplicationUser>>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AddEditUser", policy =>
                {
                    policy.RequireClaim("Add User");
                    policy.RequireClaim("Edit User");
                });
                options.AddPolicy("DeleteUser", policy => policy.RequireClaim("Delete User"));
            })
             ;
        }
    }
}
