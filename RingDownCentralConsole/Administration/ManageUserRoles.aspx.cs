using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace RingDownCentralConsole
{
    public partial class ManageUserRoles : System.Web.UI.Page
    {
        string[] rolesArray;
        MembershipUserCollection users;
        string[] usersInRole;

        public void Page_Load()
        {
            Msg.Text = "";

            if (!IsPostBack)
            {
                // Bind roles to ListBox.

                rolesArray = Roles.GetAllRoles();
                RolesListBox.DataSource = rolesArray;
                RolesListBox.DataBind();

                // Bind users to ListBox.

                users = Membership.GetAllUsers();
                UsersListBox.DataSource = users;
                UsersListBox.DataBind();
            }

            if (RolesListBox.SelectedItem != null)
            {
                // Show users in role. Bind user list to GridView.

                usersInRole = Roles.GetUsersInRole(RolesListBox.SelectedItem.Value);
                UsersInRoleGrid.DataSource = usersInRole;
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

            string[] newusers = new string[UsersListBox.GetSelectedIndices().Length];

            for (int i = 0; i < newusers.Length; i++)
            {
                newusers[i] = UsersListBox.Items[UsersListBox.GetSelectedIndices()[i]].Value;
            }


            // Add the users to the selected role.

            try
            {
                Roles.AddUsersToRole(newusers, RolesListBox.SelectedItem.Value);

                // Re-bind users in role to GridView.

                usersInRole = Roles.GetUsersInRole(RolesListBox.SelectedItem.Value);
                UsersInRoleGrid.DataSource = usersInRole;
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

            int index = Convert.ToInt32(args.CommandArgument);

            string username = ((DataBoundLiteralControl)UsersInRoleGrid.Rows[index].Cells[0].Controls[0]).Text;


            // Remove the user from the selected role.

            try
            {
                Roles.RemoveUserFromRole(username, RolesListBox.SelectedItem.Value);
            }
            catch (Exception e)
            {
                Msg.Text = "An exception of type " + e.GetType().ToString() +
                           " was encountered removing the user from the role.";
            }


            // Re-bind users in role to GridView.

            usersInRole = Roles.GetUsersInRole(RolesListBox.SelectedItem.Value);
            UsersInRoleGrid.DataSource = usersInRole;
            UsersInRoleGrid.DataBind();
        }


    }
}