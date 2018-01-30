using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RingDownCentralConsole.Models;

namespace RingDownCentralConsole
{
    public partial class ManageUserRoles : Page
    {
        private ApplicationUserManager _userManager;

        public void Page_Load()
        {
            Msg.Text = "";

            if (!IsPostBack)
            {
                // Bind roles to ListBox.

                PopulateUserManager();

                var roles = _userManager.GetRoles(User.Identity.GetUserId<int>());

                RolesListBox.DataSource = roles;
                RolesListBox.DataBind();

                // Bind users to ListBox.

                var users = _userManager.Users.ToList();
                UsersListBox.DataSource = users;
                UsersListBox.DataBind();
            }

            if (RolesListBox.SelectedItem != null)
            {
                // Show users in role. Bind user list to GridView.

                UsersInRoleGrid.DataSource = GetUsersInRole(RolesListBox.SelectedItem.Value);
                UsersInRoleGrid.DataBind();
            }
        }

        public void AddUsers_OnClick(object sender, EventArgs args)
        {
            PopulateUserManager();

            // Verify that a role is selected.

            if (RolesListBox.SelectedItem == null)
            {
                Msg.Text = "Please select a role.";
                return;
            }

            // Verify that at least one user is selected.

            if (UsersListBox.SelectedItem == null)
            {
                Msg.Text = "Please select one or more users.";
                return;
            }

            // Create list of users to be added to the selected role.

            var newUsers = new List<ApplicationUser>();

            for (int i = 0; i < UsersListBox.GetSelectedIndices().Length; i++)
            {
                var userName = UsersListBox.Items[UsersListBox.GetSelectedIndices()[i]].Value;
                var user = _userManager.FindByName(userName);
                newUsers.Add(user);
            }

            // Add the users to the selected role.

            try
            {
                AddUsersToRole(newUsers, RolesListBox.SelectedItem.Value);

                // Re-bind users in role to GridView.

                UsersInRoleGrid.DataSource = GetUsersInRole(RolesListBox.SelectedItem.Value);
                UsersInRoleGrid.DataBind();
            }
            catch (Exception e)
            {
                Msg.Text = e.Message;
            }
        }

        private void AddUsersToRole(IEnumerable<ApplicationUser> newusers, string role)
        {
            foreach (var user in newusers)
            {
                _userManager.AddToRole(user.Id, role);
            }
        }

        public void UsersInRoleGrid_RemoveFromRole(object sender, GridViewCommandEventArgs args)
        {
            PopulateUserManager();

            // Get the selected user name to remove.

            var index = Convert.ToInt32(args.CommandArgument);

            var userName = ((DataBoundLiteralControl)UsersInRoleGrid.Rows[index].Cells[0].Controls[0]).Text;

            var user = _userManager.FindByName(userName);

            // Remove the user from the selected role.

            try
            {
                RemoveUserFromRole(user, RolesListBox.SelectedItem.Value);
            }
            catch (Exception e)
            {
                Msg.Text = "An exception of type " + e.GetType() +
                           " was encountered removing the user from the role.";
            }

            // Re-bind users in role to GridView.

            var role = RolesListBox.SelectedItem.Value;

            PopulateUserManager();

            var usersInRole = _userManager.Users.Where(u => IsInRole(u, role));

            UsersInRoleGrid.DataSource = usersInRole;
            UsersInRoleGrid.DataBind();
        }

        private void RemoveUserFromRole(ApplicationUser user, string role)
        {
            _userManager.RemoveFromRole(user.Id, role);
        }

        private bool IsInRole(ApplicationUser u, string role)
        {
            return _userManager.IsInRole(u.Id, role);
        }

        private void PopulateUserManager()
        {
            if (_userManager == null)
                _userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        private IEnumerable<ApplicationUser> GetUsersInRole(string role)
        {
            return _userManager.Users.Where(u => IsInRole(u, role));
        }
    }
}