using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace RingDownCentralConsole
{
    public partial class CreateUserRoles : System.Web.UI.Page
    {
        string[] rolesArray;

        public void Page_Load(object sender, EventArgs args)
        {
            if (!IsPostBack)
            {
                // Bind roles to GridView.
                rolesArray = Roles.GetAllRoles();
                RolesGrid.DataSource = rolesArray;
                RolesGrid.DataBind();
            }
        }

        public void CreateRole_OnClick(object sender, EventArgs args)
        {
            string createRole = RoleTextBox.Text;

            try
            {
                if (Roles.RoleExists(createRole))
                {
                    Msg.Text = "Role '" + Server.HtmlEncode(createRole) + "' already exists. Please specify a different role name.";
                    return;
                }

                Roles.CreateRole(createRole);

                Msg.Text = "Role '" + Server.HtmlEncode(createRole) + "' created.";

                // Re-bind roles to GridView.

                rolesArray = Roles.GetAllRoles();
                RolesGrid.DataSource = rolesArray;
                RolesGrid.DataBind();
            }
            catch (Exception e)
            {
                Msg.Text = "Role '" + Server.HtmlEncode(createRole) + "' <u>not</u> created.";
                Response.Write(e.ToString());
            }

        }
    }
}