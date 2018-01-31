using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace RingDownCentralConsole.Models
{
    public class IdentityPage : Page
    {
        protected ApplicationRoleManager _roleManager;
        protected ApplicationUserManager _userManager;
        private List<ApplicationUser> _allUsers;
        private List<ApplicationRole> _allRoles;

        protected void AddUsersToRole(IEnumerable<ApplicationUser> newusers, string role)
        {
            foreach (var user in newusers)
            {
                _userManager.AddToRole(user.Id, role);
            }
        }

        protected void CreateRole(string roleName)
        {
            var role = new ApplicationRole();
            role.Name = roleName;
            var result = _roleManager.CreateAsync(role).Result;
        }

        protected List<ApplicationRole> GetAllRoles()
        {
            if (_allRoles == null)
                _allRoles = _roleManager.Roles.ToList();

            return _allRoles;
        }

        protected List<ApplicationUser> GetAllUsers()
        {
            if (_allUsers == null)
                _allUsers = _userManager.Users.ToList();

            return _allUsers;
        }

        protected List<ApplicationUser> GetUsersInRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return null;
            
            var role = GetAllRoles().FirstOrDefault(r => r.Name == roleName);

            if (role == null)
                return null;

            return role.Users.Select(ur => GetAllUsers().First(u => u.Id == ur.UserId)).ToList();
        }

        protected void RemoveUserFromRole(ApplicationUser user, string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return;

            _userManager.RemoveFromRole(user.Id, role);
        }

        protected bool RoleExists(string roleName)
        {
            return _roleManager.RoleExistsAsync(roleName).Result;
        }

        protected void PopulateRoleManager()
        {
            if (_roleManager == null)
                _roleManager = Request.GetOwinContext().Get<ApplicationRoleManager>();
        }

        protected void PopulateUserManager()
        {
            if (_userManager == null)
                _userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }
    }
}