﻿using System;
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
        string constr = ConfigurationManager.ConnectionStrings["csConsole"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            ////If NOT authenicated and NOT role admin
            //if ((!Page.User.Identity.IsAuthenticated) && (!Roles.IsUserInRole("Administrator")))
            //{
            // Response.Redirect("Login.aspx");
            //}
        }




    }
}
