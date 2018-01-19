using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RingDownCentralConsole
{
    public partial class SiteMaster : MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {           

            //if ((Page.User.Identity.IsAuthenticated) && (Roles.IsUserInRole("Administrator")) || (Roles.IsUserInRole("User")))
            //{
            //    Menu1.Visible = true;

                //if (Roles.IsUserInRole("User"))
                //{
                //Menu1.Items.Remove(Menu1.FindItem("Locations"));
                //Menu1.Items.Remove(Menu1.FindItem("Statuses"));
                //Menu1.Items.Remove(Menu1.FindItem("Reports"));
                //}

            //}
            //else
            //{
            //    Menu1.Visible = false;
            //}
        }


    }
}