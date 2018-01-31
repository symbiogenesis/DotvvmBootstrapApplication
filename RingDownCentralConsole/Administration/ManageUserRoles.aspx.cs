using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using RingDownCentralConsole.Models;

namespace RingDownCentralConsole
{
    public partial class ManageUserRoles : IdentityPage
    {
        public void Page_Load()
        {
            PopulateUserManager();

            Msg.Text = "";

            if (!IsPostBack)
            {
                // Bind roles to ListBox.
                PopulateRoles();

                // Bind users to ListBox.
                PopulateUsers();
            }

            if (RolesListBox.SelectedItem != null)
            {
                // Show users in role. Bind user list to GridView.
                PopulateRoles();
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

                PopulateUsersInRole();
            }
            catch (Exception e)
            {
                Msg.Text = e.Message;
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
            PopulateUserManager();

            PopulateUsersInRole();
        }

        private void PopulateUsers()
        {
            UsersListBox.DataSource = GetAllUsers();
            UsersListBox.DataBind();
        }

        private void PopulateRoles()
        {
            RolesListBox.DataSource = GetAllRoles();
            RolesListBox.DataBind();
        }

        private void PopulateUsersInRole()
        {
            var role = (RolesListBox.SelectedItem.Value);

            UsersInRoleGrid.DataSource = GetUsersInRole(role);
            UsersInRoleGrid.DataBind();
        }
    }
}