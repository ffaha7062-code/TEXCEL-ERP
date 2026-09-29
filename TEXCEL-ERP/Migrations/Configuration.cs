namespace TEXCEL_ERP.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using TEXCEL_ERP.Models.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<TEXCEL_ERP.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(TEXCEL_ERP.Data.ApplicationDbContext context)
        {
            // =============================================
            // SEED ROLES
            // =============================================
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string[] roles = new string[]
            {
                "SuperAdmin",
                "BranchAdmin",
                "BranchUser",
                "Manager",
                "Employee"
            };

            foreach (var roleName in roles)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    var role = new IdentityRole
                    {
                        Name = roleName
                    };
                    roleManager.Create(role);
                }
            }

            // =============================================
            // SEED DEFAULT SUPER ADMIN USER
            // =============================================
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var adminEmail = "admin@texcel.com";
            var adminUser = userManager.FindByEmail(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Super",
                    LastName = "Admin",
                    IsActive = true,
                    EmailConfirmed = true,
                    CreatedDate = DateTime.Now
                };

                var result = userManager.Create(user, "Admin@123");

                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "SuperAdmin");
                }
            }
        }
    }
}