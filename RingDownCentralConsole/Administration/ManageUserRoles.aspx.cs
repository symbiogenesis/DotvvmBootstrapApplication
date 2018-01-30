using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RingDownCentralConsole
{
    public partial class ManageUserRoles : Page
    {
        private string[] _rolesArray;
        private MembershipUserCollection _users;
        private string[] _usersInRole;

        public void Page_Load()
        {
            Msg.Text = "";

            if (!IsPostBack)
            {
                // Bind roles to ListBox.

                _rolesArray = Roles.GetAllRoles();
                RolesListBox.DataSource = _rolesArray;
                RolesListBox.DataBind();

                // Bind users to ListBox.

                _users = Membership.GetAllUsers();
                UsersListBox.DataSource = _users;
                UsersListBox.DataBind();
            }

            if (RolesListBox.SelectedItem != null)
            {
                // Show users in role. Bind user list to GridView.

                _usersInRole = Roles.GetUsersInRole(RolesListBox.SelectedItem.Value);
                UsersInRoleGrid.DataSource = _usersInRole;
                UsersInRoleGrid.DataBind();
            }
        }

        public void AddUsers_OnClick(object sender, EventArgs args)
        {
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

            var newusers = new string[UsersListBox.GetSelectedIndices().Length];

            for (int i = 0; i < newusers.Length; i++)
            {
                newusers[i] = UsersListBox.Items[UsersListBox.GetSelectedIndices()[i]].Value;
            }

            // Add the users to the selected role.

            try
            {
                Roles.AddUsersToRole(newusers, RolesListBox.SelectedItem.Value);

                // Re-bind users in role to GridView.

                _usersInRole = Roles.GetUsersInRole(RolesListBox.SelectedItem.Value);
                UsersInRoleGrid.DataSource = _usersInRole;
                UsersInRoleGrid.DataBind();
            }
            catch (Exception e)
            {
                Msg.Text = e.Message;
            }
        }

        public void UsersInRoleGrid_RemoveFromRole(object sender, GridViewCommandEventArgs args)
        {
            // Get the selected user name to remove.

            var index = Convert.ToInt32(args.CommandArgument);

            var username = ((DataBoundLiteralControl)UsersInRoleGrid.Rows[index].Cells[0].Controls[0]).Text;

            // Remove the user from the selected role.

            try
            {
                Roles.RemoveUserFromRole(username, RolesListBox.SelectedItem.Value);
            }
            catch (Exception e)
            {
                Msg.Text = "An exception of type " + e.GetType() +
                           " was encountered removing the user from the role.";
            }

            // Re-bind users in role to GridView.

            _usersInRole = Roles.GetUsersInRole(RolesListBox.SelectedItem.Value);
            UsersInRoleGrid.DataSource = _usersInRole;
            UsersInRoleGrid.DataBind();
        }
    }
}