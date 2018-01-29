using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;

namespace RingDownCentralConsole
{
    public partial class rdccReports : System.Web.UI.Page
    {
        private readonly string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            ////If authenicated and role admin
            if ((!User.Identity.IsAuthenticated) && (!User.IsInRole("Administrator")))
            {            
                Response.Redirect("/Account/Login.aspx");
            }
        }




    }
}
