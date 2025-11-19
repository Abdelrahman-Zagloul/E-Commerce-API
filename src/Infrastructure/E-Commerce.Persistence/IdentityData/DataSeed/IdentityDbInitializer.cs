using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Persistence.IdentityData.DataSeed
{
    public class IdentityDbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDbInitializer> _logger;

        public IdentityDbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<IdentityDbInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                bool usersExist = _userManager.Users.Any();
                bool rolesExist = _roleManager.Roles.Any();

                if (usersExist && rolesExist)
                {
                    _logger.LogInformation("Database already seeded.");
                    return;
                }

                if (!rolesExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (!usersExist)
                {
                    var superAdminUser = new ApplicationUser
                    {
                        UserName = "Abdelrahman",
                        DisplayName = "Abdelrahman",
                        Email = "abdelrahman@gmail.com",
                    };


                    var adminUser = new ApplicationUser
                    {
                        UserName = "Ahmed",
                        DisplayName = "Ahmed",
                        Email = "ahmed@gmail.com",
                    };

                    await _userManager.CreateAsync(superAdminUser, "superAdmin12345");
                    await _userManager.CreateAsync(adminUser, "Admin12345");

                    await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
