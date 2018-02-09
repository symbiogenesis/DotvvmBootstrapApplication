using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.AspNet.Identity;

namespace RingDownCentralConsole.Reports
{
    public partial class Report : System.Web.UI.Page
    {

        private readonly string _constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            //If authenicated and role admin
            if ((!User.Identity.IsAuthenticated) && (!User.IsInRole("Administrator")))
            {
                //Log user out (if logged in), redirect back to login.aspx
                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Response.Redirect("/Account/Login.aspx");
            }

        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.Msg2.Text = "";
            this.Msg.Text = "";
            this.txtEndDate.Text = "";
            this.txtStartDate.Text = "";
            GridView1.DataBind();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            var sortExpression = e.SortExpression;
            var direction = string.Empty;

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

            DataTable table = this.GetData();
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
            // specify the data source for the GridView
            GridView1.DataSource = this.GetData();
            // bind the data now
            GridView1.DataBind();
        }


        private DataTable GetData()
        {
            var table = new DataTable();
            // get the connection
            using (var conn = new SqlConnection(_constr))
            {
                // write the sql statement to execute
                const string sql = "SELECT Locations.Id AS LocationID, Locations.Code AS Code, Locations.Name AS LocationName, RecordedDate, " +
                                                "Statuses.Name AS Status, Statuses.Image, Locations.IsActive " +
                                                "FROM Statuses INNER JOIN(Locations INNER JOIN LocationStatuses ON Locations.Id = LocationStatuses.LocationId) " +
                                                "ON Statuses.Id = LocationStatuses.StatusId " +
                                                "WHERE Locations.IsActive=1 ";

                // instantiate the command object to fire
                using (var cmd = new SqlCommand(sql, conn))
                {
                    // get the adapter object and attach the command object to it
                    using (var ad = new SqlDataAdapter(cmd))
                    {
                        // fire Fill method to fetch the data and fill into DataTable
                        ad.Fill(table);
                    }
                }
            }
            return table;
        }

              

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
            BindData();
           
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {  
            
            using (var con = new SqlConnection(_constr))
            {
                // using (var cmd = new SqlCommand("SELECT OrderID, OrderDate, ShipName, ShipCity FROM Orders WHERE OrderDate BETWEEN @From AND @To", con))
                using (var cmd = new SqlCommand("SELECT Locations.Id AS LocationID, Locations.Code AS Code, Locations.Name AS LocationName, RecordedDate, " +
                                                "Statuses.Name AS Status, Statuses.Image, Locations.IsActive " +
                                                "FROM Statuses INNER JOIN(Locations INNER JOIN LocationStatuses ON Locations.Id = LocationStatuses.LocationId) " +
                                                "ON Statuses.Id = LocationStatuses.StatusId " +
                                                "WHERE (CAST(RecordedDate As DATE) >= @From) And (CAST(RecordedDate As DATE) <= @To) And Locations.IsActive=1 " +
                                                "GROUP BY Locations.Id, Locations.Name, Locations.Code, Statuses.Name, Statuses.Image, Locations.IsActive, RecordedDate " +                                             
                                                "ORDER BY RecordedDate DESC, Locations.Name DESC", con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {

                            var start = Convert.ToDateTime(this.txtStartDate.Text);
                            var startDate = String.Format("{0:MM/dd/yyyy}", start);

                            var end = Convert.ToDateTime(this.txtEndDate.Text);
                            var endDate = String.Format("{0:MM/dd/yyyy}", end);

                            cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(this.txtStartDate.Text, new CultureInfo("en-US")));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(this.txtEndDate.Text, new CultureInfo("en-US")));
                        var ds = new DataSet();
                        da.Fill(ds);
                        GridView1.DataSource = ds;
                        GridView1.DataBind();
                        con.Close();

                        if (ds.Tables[0].Rows.Count == 0)
                            {  
                            this.Msg.Text = "No Records Found";
                            this.Msg2.Text = "";
                            }
                        else
                            {
                                this.Msg2.Text = "Results for " + startDate + " to " + endDate;
                                this.Msg.Text = "";
                               
                            }
                        }
                }
                }
            }
        }
    }
}