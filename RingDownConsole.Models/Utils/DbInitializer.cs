using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RingDownConsole.Interfaces;
using RingDownConsole.Models.Enums;
using RingDownConsole.Utils;

namespace RingDownConsole.Models.Utils
{
    public class DbInitializer : IDbInitializer
    {
        private readonly RingDownConsoleDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _appSettings;

        public DbInitializer(
            RingDownConsoleDbContext context,
            IOptionsSnapshot<AppSettings> appSettings,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
        }

        public static bool Initialized { get; private set; }

        //This example just creates an Administrator role and one Admin users
        public async Task Initialize()
        {
            //create database schema if none exists
            _context.Database.EnsureCreated();
            //_context.Database.Migrate();

            //check if data already initialized
            if (_context.Statuses.Any())
            {
                Initialized = true;
                return;
            }

            await CreateUsersAndRoles();

            await GenerateData();

            Initialized = true;
        }

        private async Task GenerateData()
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                //Generate Data
                DataGenerator.Generate(500);
                await _context.Locations.AddRangeAsync(DataGenerator.Locations.ToList());
                await _context.Statuses.AddRangeAsync(DataGenerator.Statuses.ToList());
                await _context.LocationStatuses.AddRangeAsync(DataGenerator.LocationStatuses.ToList());

                //Save changes
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
        }

        private async Task CreateUsersAndRoles()
        {
            //Create Roles
            await CreateRoles(new string[] { nameof(Roles.Administrator), nameof(Roles.User) });

            //Create the default Admin account and apply the Administrator role
            await CreateUser("admin", "z0mgChangethis!", true);
            await CreateUser("user", "uSer1!", false);

            _context.SaveChanges();

            _userManager.Dispose();
            _roleManager.Dispose();
        }

        private async Task CreateRoles(string[] names)
        {
            foreach (var name in names)
            {
                if (!await _roleManager.RoleExistsAsync(name))
                {
                    await _roleManager.CreateAsync(new IdentityRole(name));
                }
            }
        }

        private async Task CreateUser(string userName, string password, bool isAdmin)
        {
            if (await _userManager.FindByNameAsync(userName) != null)
                return;

            var userToCreate = new IdentityUser { UserName = userName, EmailConfirmed = true };
            await _userManager.CreateAsync(userToCreate, password);

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return;

            string[] roles;

            if (isAdmin)
                roles = new string[] { nameof(Roles.Administrator), nameof(Roles.User) };
            else
                roles = new string[] { nameof(Roles.User) };

            await _userManager.AddToRolesAsync(user, roles);
        }
    }
}