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
            return _roleManager.Roles.ToList();
        }

        protected List<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        protected IEnumerable<ApplicationUser> GetUsersInRole(string role)
        {
            return _userManager.Users.Where(u => IsInRole(u, role));
        }

        protected bool IsInRole(ApplicationUser u, string role)
        {
            return _userManager.IsInRole(u.Id, role);
        }

        protected void RemoveUserFromRole(ApplicationUser user, string role)
        {
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