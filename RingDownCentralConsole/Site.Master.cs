using Microsoft.AspNet.Identity;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.Providers.Entities;

namespace RingDownCentralConsole
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string) ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string) ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            MenuItem login = Menu1.FindItem("Login");
            MenuItem locations = Menu1.FindItem("Locations");
            MenuItem statuses = Menu1.FindItem("Statuses");
            MenuItem reports = Menu1.FindItem("Reports");
            //MenuItem managePassword = Menu1.FindItem("ManagePassword");
            MenuItem register = Menu1.FindItem("Register");


            if (!Page.User.Identity.IsAuthenticated)
                {
                    Menu1.Visible = false;
                }
                else
                {
                    Menu1.Visible = true;

                      if (Page.User.IsInRole("Administrator"))
                        {
                            if (login != null)
                            {
                                Menu1.Items.Remove(Menu1.FindItem("Login"));
                            }
                        }
                          else
                                if (Page.User.IsInRole("User"))
                                    {
                                        if (login != null)
                                        {
                                            Menu1.Items.Remove(Menu1.FindItem("Login"));
                                        }
                                       
                                        if (locations != null)
                                        {
                                            Menu1.Items.Remove(Menu1.FindItem("Locations"));
                                        }

                                        if (statuses != null)
                                        {
                                            Menu1.Items.Remove(Menu1.FindItem("Statuses"));
                                        }

                                        if (reports != null)
                                        {
                                            Menu1.Items.Remove(Menu1.FindItem("Reports"));
                                        }

                                        if (register != null)
                                        {
                                            Menu1.Items.Remove(Menu1.FindItem("Register"));
                                        }

                                    }

                                     else
                                        if (Page.User.IsInRole(""))
                                            {
                                                 Response.Redirect("/Error/AuthenticationMenuError.aspx");

                                            }
                      
            }
                      

        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Response.Redirect("/Account/Login.aspx");
        }


        //protected void Menu1_DataBound(object sender, EventArgs e)
        //{
        //    MenuItem loginMenuItem =
        //        new MenuItem(
        //            LoginStatus1.LogoutText,
        //            "logout",
        //            LoginStatus1.LogoutImageUrl,
        //            ClientScript.GetPostBackClientHyperlink(LoginStatus1.Controls[0], null));

        //    Menu1.Items.Add(loginMenuItem);
        //}


       



    }
}