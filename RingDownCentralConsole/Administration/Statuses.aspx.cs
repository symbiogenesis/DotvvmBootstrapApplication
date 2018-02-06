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
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace RingDownCentralConsole
{
    public partial class Statuses : System.Web.UI.Page
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
                //Send user back to main console page, because user is not an "Administrator" role
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            var sortExpression = e.SortExpression;
            var direction = string.Empty;
            var strQuery = "SELECT * from Statuses Where IsActive=1";
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
            var strQuery = "SELECT * from Statuses Where IsActive=1";
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
                       
               
      
            protected void OnPaging(object sender, GridViewPageEventArgs e)
            {
            BindData();
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
            }

            protected void EditStatus(object sender, GridViewEditEventArgs e)
            {
                GridView1.EditIndex = e.NewEditIndex;
                BindData();
            }

            protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
            {
                GridView1.EditIndex = -1;
                BindData();
            }


    }
}
