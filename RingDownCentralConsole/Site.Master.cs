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

            if ((Page.User.Identity.IsAuthenticated) && (Roles.IsUserInRole("Administrator")) || (Roles.IsUserInRole("User")))
            {
                Menu1.Visible = true;

                if (Page.User.IsInRole("User"))
                {
                    // Menu1.Items[1].Enabled = false;
                    //Menu1.Items.RemoveAt(1);
                    //Menu1.Items.RemoveAt(3);

                    
                    //MenuItem dboard = Menu1.FindItem(@"AdminDasboard");
                    //dboard.Items
                    //// Remove the Movie submenu item.
                    //if (dboard != null)
                    //{
                        
                    //}



                }

                


            }
            else
            {
                Menu1.Visible = false;
            }
            

            
        }

       
    }
}