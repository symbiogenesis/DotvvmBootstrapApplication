using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity.Owin;
using RingDownCentralConsole.Models;

namespace RingDownCentralConsole
{
    public partial class CreateUserRoles : Page
    {
        private ApplicationRoleManager _roleManager;

        public void Page_Load(object sender, EventArgs args)
        {
            if (!IsPostBack)
            {
                PopulateRoleManager();

                // Bind roles to GridView.
                RolesGrid.DataSource = GetAllRoles();
                RolesGrid.DataBind();
            }
        }

        public void CreateRole_OnClick(object sender, EventArgs args)
        {
            var roleName = RoleTextBox.Text;

            try
            {
                if (RoleExists(roleName))
                {
                    Msg.Text = "Role '" + Server.HtmlEncode(roleName) + "' already exists. Please specify a different role name.";
                    return;
                }

                CreateRole(roleName);

                Msg.Text = "Role '" + Server.HtmlEncode(roleName) + "' created.";

                // Re-bind roles to GridView.

                RolesGrid.DataSource = GetAllRoles();
                RolesGrid.DataBind();
            }
            catch (Exception e)
            {
                Msg.Text = "Role '" + Server.HtmlEncode(roleName) + "' <u>not</u> created.";
                Response.Write(e.ToString());
            }
        }

        private object GetAllRoles()
        {
            return _roleManager.Roles;
        }

        private bool RoleExists(string roleName)
        {
            return _roleManager.RoleExistsAsync(roleName).Result;
        }

        private void CreateRole(string roleName)
        {
            var role = new ApplicationRole();
            role.Name = roleName;
            var result = _roleManager.CreateAsync(role).Result;
        }

        private void PopulateRoleManager()
        {
            if (_roleManager == null)
                _roleManager = Request.GetOwinContext().Get<ApplicationRoleManager>();
        }
    }
}