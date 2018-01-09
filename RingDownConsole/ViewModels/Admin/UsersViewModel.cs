using System;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RingDownConsole.Interfaces;
using RingDownConsole.Models;
using RingDownConsole.Models.Enums;
using RingDownConsole.Utils.Extensions;

namespace RingDownConsole.ViewModels.Admin
{
    public class UsersViewModel : AdminViewModel<User>
    {
        private readonly UserManager<User> _userManager;

        public string PasswordToChange { get; set; }

        public UsersViewModel(
            IOptionsSnapshot<AppSettings> appSettings,
            UserManager<User> userManager) : base(appSettings)
        {
            _userManager = userManager;
        }

        public async Task CancelEdit(User item)
        {
            PasswordToChange = null;

            Data.RowEditOptions.EditRowId = null;

            if (!item.IsSaved)
                await Delete(item);

            await Data.RequestRefreshAsync(true);
        }

        public async Task Delete(User item)
        {
            if (!item.IsSaved)
            {
                await Delete(item);
                return;
            }

            var user = await _userManager.FindByIdAsync(item.Id.ToString());
            await _userManager.UpdateSecurityStampAsync(user);
            await _userManager.DeleteAsync(user);
            await Data.RequestRefreshAsync(true);
        }

        public void Edit(User item)
        {
            PasswordToChange = null;
            Data.RowEditOptions.EditRowId = item.Id;
        }

        public async Task Update(User item)
        {
            User existingUser = null;

            if (item.Id != 0)
                existingUser = await _userManager.FindByIdAsync(item.Id.ToString());

            if (existingUser == null)
            {
                await CreateUser(item.UserName, PasswordToChange, item.IsAdmin);
            }
            else
            {
                await UpdateUser(item.UserName, PasswordToChange, item.IsAdmin, existingUser);
            }

            PasswordToChange = null;

            await Data.RequestRefreshAsync(true);
            Data.RowEditOptions.EditRowId = null;
        }

        private async Task CreateUser(string userName, string password, bool isAdmin)
        {
            var userToCreate = new User { UserName = userName, Email = userName, EmailConfirmed = true };
            await _userManager.CreateAsync(userToCreate, password);

            var user = await _userManager.FindByNameAsync(userName);

            var roles = GetRoles(isAdmin);

            await _userManager.AddToRolesAsync(user, roles);
        }

        private async Task UpdateUser(string userName, string password, bool isAdmin, User user)
        {
            if (user == null)
            {
                ShowError($"User: {userName} not found. Cannot update.");
                return;
            }

            user.Name = userName;
            user.UserName = userName;

            if (!string.IsNullOrWhiteSpace(password))
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);

            await _userManager.UpdateAsync(user);

            await _userManager.RemoveFromRolesAsync(user, GetRoles(true));

            var roles = GetRoles(isAdmin);

            foreach (var role in roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            await Data.RequestRefreshAsync();
        }

        private string[] GetRoles(bool isAdmin)
        {
            if (isAdmin)
                return new string[] { nameof(Roles.Administrator), nameof(Roles.User) };
            else
                return new string[] { nameof(Roles.User) };
        }

        protected IQueryable<User> GetItems(IQueryable<User> items)
        {
            foreach (var item in items)
            {
                item.IsAdmin = _userManager.IsInRoleAsync(item, nameof(Roles.Administrator)).Result;
            }

            return items;
        }

        private GridViewDataSetLoadedData<User> GetData(IGridViewDataSetLoadOptions options)
        {
            if (_userManager.Users == null)
                throw new Exception($"{typeof(User).Name} data could not be retrieved from server");

            var items = GetItems(_userManager.Users);

            if (options.SortingOptions.SortExpression == null)
            {
                options.SortingOptions.SortExpression = nameof(IAdminLookup.Name);
                options.SortingOptions.SortDescending = false;
            }

            return items.GetDataFromQueryable(options);
        }

        public virtual void AddRecord()
        {
            if (Data.RowEditOptions.EditRowId != null)
            {
                ErrorMessage = "Finish editing field";
                ErrorShow = true;
                return;
            }

            var newRecord = new User
            {
                Id = Data.Items.Max(u => u.Id) + 1,
                IsSaved = false
            };
            Data.Items.Add(newRecord);
            Data.Items.MoveItemAtIndexToFront(Data.Items.Count - 1);

            Data.RowEditOptions.EditRowId = newRecord.Id;
        }

        public override Task Init()
        {
            // NOTE: You can also create the DataSet with factory.
            // Just call static Create with delegate and pagesize.
            Data = GridViewDataSet.Create(GetData, pageSize: _appSettings.PageSize);
            Data.RowEditOptions = new RowEditOptions { PrimaryKeyPropertyName = nameof(IIdentifiable.Id) };

            return base.Init();
        }
    }
}
