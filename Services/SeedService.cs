using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReelJunkies.Data;
using ReelJunkies.Models.Database;
using ReelJunkies.Models.Settings;
using System.Linq;
using System.Threading.Tasks;

namespace ReelJunkies.Services
{
    public class SeedService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //using IOPTIONS bec we are using configured instance of the appSettings
        public SeedService(IOptions<AppSettings> appSettings,
                                ApplicationDbContext dbContext,
                                RoleManager<IdentityRole> roleManager,
                                UserManager<IdentityUser> userManager)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManagedDataAsync()
        {
            await UpdateDatabaseAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedCollections();
        }

        private async Task UpdateDatabaseAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (_dbContext.Roles.Any())
            {
                return;
            }

            var adminRole = _appSettings.ReelJunkiesSettings.DefaultCredentials.Role;

            await _roleManager.CreateAsync(new IdentityRole(adminRole));
        }


        private async Task SeedUsersAsync()
        {
            if (_dbContext.Users.Any())
            {
                return;
            }

            var credentials = _appSettings.ReelJunkiesSettings;
            var newUser = new IdentityUser()
            {
                Email = credentials.DefaultCredentials.Email,
                UserName = credentials.DefaultCredentials.Email,
                EmailConfirmed = true
            };

            var demoUser = new IdentityUser()
            {
                Email = credentials.DemoUser.Email,
                UserName = credentials.DemoUser.Email,
                EmailConfirmed = true

            };
            await _userManager.CreateAsync(newUser, credentials.DefaultCredentials.Password);
            await _userManager.CreateAsync(demoUser, credentials.DemoUser.Password);
            await _userManager.AddToRoleAsync(newUser, credentials.DefaultCredentials.Role);
        }


        private async Task SeedCollections()
        {
            if (_dbContext.Collection.Any())
            {
                return;
            }

            _dbContext.Add(new Collection()
            {
                Name = _appSettings.ReelJunkiesSettings.DefaultCollection.Name,
                Description = _appSettings.ReelJunkiesSettings.DefaultCollection.Description,
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}
