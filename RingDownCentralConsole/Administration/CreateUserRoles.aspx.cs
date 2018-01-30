using System;
using System.Web.Security;
using System.Web.UI;

namespace RingDownCentralConsole
{
    public partial class CreateUserRoles : Page
    {
        private string[] _rolesArray;

        public void Page_Load(object sender, EventArgs args)
        {
            if (!IsPostBack)
            {
                // Bind roles to GridView.
                _rolesArray = Roles.GetAllRoles();
                RolesGrid.DataSource = _rolesArray;
                RolesGrid.DataBind();
            }
        }

        public void CreateRole_OnClick(object sender, EventArgs args)
        {
            var roleName = RoleTextBox.Text;

            try
            {
                if (Roles.RoleExists(roleName))
                {
                    Msg.Text = "Role '" + Server.HtmlEncode(roleName) + "' already exists. Please specify a different role name.";
                    return;
                }

                Roles.CreateRole(roleName);

                Msg.Text = "Role '" + Server.HtmlEncode(roleName) + "' created.";

                // Re-bind roles to GridView.

                _rolesArray = Roles.GetAllRoles();
                RolesGrid.DataSource = _rolesArray;
                RolesGrid.DataBind();
            }
            catch (Exception e)
            {
                Msg.Text = "Role '" + Server.HtmlEncode(roleName) + "' <u>not</u> created.";
                Response.Write(e.ToString());
            }
        }
    }
}