﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RingDownCentralConsole.Messages
{
    public partial class AccountReview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {  
           
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            //Redirect new user back to main page/Login.aspx
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}