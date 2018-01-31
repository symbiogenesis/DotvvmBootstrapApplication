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
            PopulateRoleManager();

            Msg.Text = "";

            if (!IsPostBack)
            {
                // Bind roles to ListBox.
                BindRoles();

                // Bind users to ListBox.
                BindUsers();
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

                BindUsersInRole(RolesListBox.SelectedItem.Value);
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

            var userName = ((DataBoundLiteralControl)UsersInRoleGrid.Rows[index].Cells[0].Controls[0]).Text.Trim('\r').Trim('\n').Trim();

            var user = _userManager.FindByName(userName);

            // Remove the user from the selected role.

            var roleName = RolesListBox.SelectedItem.Text;

            try
            {
                RemoveUserFromRole(user, roleName);
            }
            catch (Exception e)
            {
                Msg.Text = "An exception of type " + e.GetType() +
                           " was encountered removing the user from the role.";
            }

            BindUsersInRole(roleName);
        }

        protected void RolesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var rolesListBox = ((ListBox) sender);
            var roleIndex = rolesListBox.SelectedIndex;
            var rolesList = GetAllRoles();

            if (roleIndex >= 0 && roleIndex < rolesList.Count)
            {
                var role = rolesList[roleIndex];
                BindUsersInRole(role?.Name);
            }
            else
            {
                RolesListBox.DataSource = null;
            }
        }

        private void BindUsers()
        {
            UsersListBox.DataSource = GetAllUsers();
            UsersListBox.DataBind();
        }

        private void BindRoles()
        {
            RolesListBox.DataSource = GetAllRoles();
            RolesListBox.DataBind();
        }

        private void BindUsersInRole(string role)
        {
            UsersInRoleGrid.DataSource = GetUsersInRole(role);
            UsersInRoleGrid.DataBind();
        }
    }
}