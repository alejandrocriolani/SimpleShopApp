using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Almacen.Models
{
    public static class IdentityDataInitializer
    {
        public static async Task SeedRolesAsync (RoleManager<IdentityRole> roleManager)
        {
            if(!roleManager.RoleExistsAsync("admin").Result)
            {
                IdentityRole role = new IdentityRole("admin");
                await roleManager.CreateAsync(role);
            }
            if(!roleManager.RoleExistsAsync("client").Result)
            {
                IdentityRole role = new IdentityRole("client");
                await roleManager.CreateAsync(role);
            }
            if (!roleManager.RoleExistsAsync("employee").Result)
            {
                IdentityRole role = new IdentityRole("employee");
                await roleManager.CreateAsync(role);
            }
            if(!roleManager.RoleExistsAsync("supplier").Result)
            {
                IdentityRole role = new IdentityRole("supplier");
                await roleManager.CreateAsync(role);
            }
        }
    }
}
