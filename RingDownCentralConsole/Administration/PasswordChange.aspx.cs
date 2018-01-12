using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;


namespace RingDownCentralConsole.Administration
{
    public partial class PasswordChange : System.Web.UI.Page
    {
        public void ChangePassword_OnClick(object sender, EventArgs args)
        {
            // Update the password.

            MembershipUser u = Membership.GetUser(User.Identity.Name);

            try
            {
                if (u.ChangePassword(OldPasswordTextbox.Text, PasswordTextbox.Text))
                {
                    Msg.Text = "Password changed.";
                }
                else
                {
                    Msg.Text = "Password change failed. Please re-enter your values and try again.";
                }
            }
            catch (Exception e)
            {
                Msg.Text = "An exception occurred: " + Server.HtmlEncode(e.Message) + ". Please re-enter your values and try again.";
            }
        }

    }
}