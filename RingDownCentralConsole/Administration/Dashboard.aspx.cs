using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;




namespace RingDownCentralConsole
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();
        MembershipUser u;
  

        protected void Page_Load(object sender, EventArgs e)
        {         

            if (!IsPostBack)
                {
                    //BindData();
                    BindData();
                }            
    }


        private void BindData()
        {
            string strQuery = "SELECT aspnet_Membership.UserId, Email, UserName, FirstName, LastName, RoleName, " +
                              "IsApproved, IsLockedOut, LastLockoutDate, LastActivityDate, " +
                              "LastLoginDate, LastPasswordChangedDate, CreateDate " +
                              "FROM (aspnet_UsersInRoles INNER JOIN ((aspnet_Membership INNER JOIN User_Details " +
                              "ON aspnet_Membership.UserId = User_Details.UserId) " +
                              "INNER JOIN aspnet_Users ON aspnet_Membership.UserId = aspnet_Users.UserId) " +
                              "ON aspnet_UsersInRoles.UserId = User_Details.UserId) INNER JOIN aspnet_Roles " +
                              "ON aspnet_UsersInRoles.RoleId = aspnet_Roles.RoleId  Order by FirstName, LastName";




            SqlCommand cmd = new SqlCommand(strQuery);
            GridView1.DataSource = GetData(cmd);
            GridView1.DataBind();
        }

       

        private DataTable GetData(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            con.Open();
            sda.SelectCommand = cmd;
            sda.Fill(dt);
            return dt;
        }

        protected void ResetPassword_OnClick(object sender, EventArgs args)
        {            
            LinkButton lnkRstPasword = (LinkButton)sender;
           string  username = lnkRstPasword.CommandArgument;
            string newPassword;

            u = Membership.GetUser(username, false);         


            Msg.Text = "Username " + Server.HtmlEncode(username) + " not found. Please check the value and re-enter.";

            if (u == null)
            {
                Msg.Text = "Username " + Server.HtmlEncode(username) + " not found. Please check the value and re-enter.";
                return;
            }

            try
            {
                Random random = new Random();
                int newNum = random.Next(1, 100000000);
                newPassword = "Welcome" + Convert.ToString(newNum) + "#";              
                u.ChangePassword(u.ResetPassword(), newPassword);               

                //newPassword = u.GetPassword();
            }
            catch (MembershipPasswordException e)
            {
                Msg.Text = "Invalid password answer. Please re-enter and try again.";
                return;
            }
            catch (Exception e)
            {
                Msg.Text = e.Message;
                return;
            }

            if (newPassword != null)
            {
                Msg.Text = "The password for " + username + " is: " + Server.HtmlEncode(newPassword);            
            }
            else
            {
                Msg.Text = "Password reset failed. Please re-enter your values and try again.";
            }

           
        }


        protected void EditUser(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindData();
        }


        protected void ActivateDeactivate_OnClick(object sender, EventArgs args)
        {
            
                LinkButton lnkActDeactivate = (LinkButton)sender;
                string username = lnkActDeactivate.CommandArgument;
                u = Membership.GetUser(username, false);
                


            string MsgHolder = "";

                //If user is already activiated, then de-activate user.  Alternatively,
                //if user is de-activated, then activate user
             try
            { 
                if (u.IsApproved == true)
                {
                    u.IsApproved = false;
                    MsgHolder = " is Inactive.";
                    
                }
                else
                {
                    u.IsApproved = true;
                    MsgHolder = " is Active.";
                }
                Membership.UpdateUser(u);
                Msg.Text = u + MsgHolder;
                BindData();
                //Response.Redirect(Request.Url.AbsoluteUri);           
            }
            catch
            {
                Msg.Text = "Error in ActivateDeactivate_OnClickv Routine";
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Check if row is data row, not header, footer etc.
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get value of third column. Index is zero based, to 
                // get text of third column we use Cells[2].Text

                //string ActiveStatus = cell.Text.Trim();
                // If value is greater of 10, change format

                TableCell cell = e.Row.Cells[3];
                Label Act = e.Row.Cells[3].FindControl("lblIsApproved") as Label;
                string ActiveStatus = Act.Text;
                
                if (ActiveStatus.Contains("Active"))
                {
                    // Use this syntax to change format of single cell
                    cell.BackColor = Color.LightGreen;
                }
                else
                {
                    cell.BackColor = Color.LightSalmon;
                }
            }
        }

     
        protected void Roles_OnClick(object sender, EventArgs args)
        {
            LinkButton lnkRoles = (LinkButton)sender;          
            string username = lnkRoles.CommandArgument;
            u = Membership.GetUser(username, false);
            string[] rolesuserbelongto = Roles.GetRolesForUser(username);
            string MsgHolder = "";         

            try
            {

                if (rolesuserbelongto[0].Length.Equals(0))
                {
                    Roles.AddUserToRole(u.ToString(), "User");
                    MsgHolder = "User";
                }

                if (rolesuserbelongto[0].Equals("User"))
                   {
                    Roles.RemoveUserFromRole(u.ToString(), "User");
                    Roles.AddUserToRole(u.ToString(), "Administrator");
                    MsgHolder = "Administrator";

                }

                if (rolesuserbelongto[0].Equals("Administrator"))
                {
                    Roles.RemoveUserFromRole(u.ToString(), "Administrator");
                    Roles.AddUserToRole(u.ToString(), "User");
                    MsgHolder = "User";
                }

                Msg.Text = u + " assigned to " + MsgHolder + " role.";
                BindData();
            }
            catch
            {
                Msg.Text = " Error in Roles_OnClick Routine ";
            }

        }


        protected void UpdateUser(object sender, GridViewUpdateEventArgs e)
        {
            string UserID = ((Label)GridView1.Rows[e.RowIndex].FindControl("UserID")).Text;
            string UserName = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblUserName")).Text;
            string Email = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtEmail")).Text;
            string FirstName = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtFirstName")).Text;
            string LastName = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtLastName")).Text;      
          
            string constr = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("aspnet_UpdateMembershipTables"))
                {
                   
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(UserID);
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;


                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        try
                        { 
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        }
                        catch (SqlException ex)
                        {

                            Msg.Text = "Command Type Error" + ex;
                        }
                        using (DataTable dt = new DataTable())
                        {

                            try { 
                            sda.Fill(dt);
                            GridView1.EditIndex = -1;
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                            BindData();
                            Msg.Text = "Record updated successfully";
                            }
                            catch (SqlException ex)
                            {
                                Msg.Text = "GridView /Data Fill Error" + ex;

                            }
                        }
                    }
                }


                
            }        
            

            //role code here
                
           
        }


    }
}