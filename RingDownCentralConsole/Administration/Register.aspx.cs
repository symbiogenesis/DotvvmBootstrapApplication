using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Web.Security;

namespace RingDownCentralConsole
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
        {
           

                // Get the UserId of the just-added user
                MembershipUser newUser = Membership.GetUser(CreateUserWizard1.UserName);

                Guid newUserId = (Guid)newUser.ProviderUserKey;

                //Get Profile Data Entered by user in CUW control
                String FirstName = ((TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("FirstName")).Text;

                String LastName = ((TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("LastName")).Text;
                // Insert a new record into User_Profile

                // Get your Connection String from the web.config. MembershipConnectionString is the name I have in my web.config

                string connectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
                string insertSql = "INSERT INTO User_Details(UserId,FirstName, LastName) VALUES(@UserId, @FirstName, @LastName)";

                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {

                    myConnection.Open();
                    SqlCommand myCommand = new SqlCommand(insertSql, myConnection);
                    myCommand.Parameters.AddWithValue("@UserId", newUserId);
                    myCommand.Parameters.AddWithValue("@FirstName", FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", LastName);
                     myCommand.ExecuteNonQuery();
                    myConnection.Close();

                }

            
        }
    }
}