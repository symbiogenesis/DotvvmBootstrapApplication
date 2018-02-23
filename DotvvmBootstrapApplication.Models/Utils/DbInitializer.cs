using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using DotvvmBootstrapApplication.Interfaces;
using DotvvmBootstrapApplication.Models.Enums;
using DotvvmBootstrapApplication.Utils;

namespace DotvvmBootstrapApplication.Models.Utils
{
    public class DbInitializer : IDbInitializer
    {
        private readonly BootstrapDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppSettings _appSettings;

        public DbInitializer(
            BootstrapDbContext context,
            IOptionsSnapshot<AppSettings> appSettings,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
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
            if (_context.ExampleTable.Any())
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
                await _context.ExampleTable.AddRangeAsync(DataGenerator.ExampleRecords.ToList());

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
                    await _roleManager.CreateAsync(new Role(name));
                }
            }
        }

        private async Task CreateUser(string userName, string password, bool isAdmin)
        {
            if (await _userManager.FindByNameAsync(userName) != null)
                return;

            var userToCreate = new User { UserName = userName, EmailConfirmed = true };
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