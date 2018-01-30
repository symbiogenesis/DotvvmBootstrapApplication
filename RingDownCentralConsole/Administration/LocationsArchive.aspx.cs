using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace RingDownCentralConsole
{
    public partial class LocationsArchive : System.Web.UI.Page
    {
        private readonly string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            ////If authenicated and role admin
            if ((User.Identity.IsAuthenticated) && (User.IsInRole("Administrator")))
            {
                if (!IsPostBack)
                {
                    BindData();
                }
            }
            else
            {
                //Log user out (if logged in), redirect back to login.aspx
                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Response.Redirect("/Account/Login.aspx");
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            var sortExpression = e.SortExpression;
            var direction = string.Empty;
            var strQuery = "SELECT * from Locations Where IsActive=1";
            var cmd = new SqlCommand(strQuery);

            if (SortDirection == SortDirection.Ascending)
            {
                SortDirection = SortDirection.Descending;
                direction = " DESC";
            }
            else
            {
                SortDirection = SortDirection.Ascending;
                direction = " ASC";
            }

            DataTable table = this.GetData(cmd);
            table.DefaultView.Sort = sortExpression + direction;
            GridView1.DataSource = table;
            GridView1.DataBind();
        }

        public SortDirection SortDirection

        {
            get
            {
                if (ViewState["SortDirection"] == null)
                {
                    ViewState["SortDirection"] = SortDirection.Ascending;
                }
                return (SortDirection) ViewState["SortDirection"];
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }





        private void BindData()
        {
            var strQuery = "SELECT * from Locations Where IsActive=0";
            var cmd = new SqlCommand(strQuery);          
            GridView1.DataSource = GetData(cmd);
            GridView1.DataBind();           
        }


        private DataTable GetData(SqlCommand cmd)
        {
            var dt = new DataTable();
            var con = new SqlConnection(constr);
            var sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            con.Open();
            sda.SelectCommand = cmd;
            sda.Fill(dt);
            return dt;
        }



               
        protected void ActivateLocation(object sender, EventArgs e)
        {         
            
            using (var con = new SqlConnection(constr))
            {
                var lnkRemove = (LinkButton)sender;
                try
                {
                    var cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update Locations set IsActive=@IsActive Where Id=@Id;" +
                     "Select * from Locations Where IsActive=0";
                    cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = lnkRemove.CommandArgument;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 1;

                    GridView1.EditIndex = -1;
                    GridView1.DataSource = GetData(cmd);
                    GridView1.DataBind();
                    BindData();                   
                }
                catch (Exception ex)
                {
                    /*Handle error*/
                    Msg.Text = "Connection Error in ActivateLocation module" + ex;
                }

            }
            
           
        }



        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            BindData();
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }


     
       

       



       
    }
}