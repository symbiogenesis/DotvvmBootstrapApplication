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

namespace RingDownCentralConsole
{
    public partial class LocationsArchive : System.Web.UI.Page
    {
        private readonly string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {

            ////If authenicated and role admin
            //if ((Page.User.Identity.IsAuthenticated) && (Roles.IsUserInRole("Administrator")))
            //{
                if (!IsPostBack)
                {
                    BindData();
                }
            //}
            //else
            //{
            //    // Response.Redirect("Login.aspx");
            //}
        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string direction = string.Empty;
            string strQuery = "SELECT * from Locations Where IsActive=1";
            SqlCommand cmd = new SqlCommand(strQuery);

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
            string strQuery = "SELECT * from Locations Where IsActive=0";
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



               
        protected void ActivateLocation(object sender, EventArgs e)
        {         
            
            using (SqlConnection con = new SqlConnection(constr))
            {
                LinkButton lnkRemove = (LinkButton)sender;
                try
                {
                    SqlCommand cmd = new SqlCommand();
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