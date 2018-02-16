using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;


namespace RingDownCentralConsole.Reports
{
    public partial class Report2 : Page
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


            if (!IsPostBack)
            {

                ddlLocations.AppendDataBoundItems = true;
                var strQuery = "Select Id, Name, Name+'  |  '+Code As LocationCode from Locations";
                var con = new SqlConnection(_constr);
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strQuery;
                cmd.Connection = con;

                try
                {
                    con.Open();
                    ddlLocations.DataSource = cmd.ExecuteReader();
                    ddlLocations.DataTextField = "LocationCode";
                    ddlLocations.DataValueField = "Id";
                    ddlLocations.DataBind();
                }
                catch (Exception ex)
                {
                    Msg.Text = "Error in Load Routine " + ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }


            }
        }


        protected void ddlLocations_SelectedIndexChanged(object sender, EventArgs e)
        {

            // ddlStatuses.Items.Clear();
            //ddlStatuses.Items.Add(new ListItem("--Select Status--", ""));
            ddlStatuses.AppendDataBoundItems = true;
            var strQuery = "SELECT Distinct Statuses.Id As StatusID, Statuses.Name As StatusName " +
                              "FROM Statuses " +
                              "INNER JOIN (Locations INNER JOIN LocationStatuses ON " +
                              "Locations.Id = LocationStatuses.LocationId) ON Statuses.Id = LocationStatuses.StatusId " +
                              "WHERE LocationID=@LocationID  ORDER BY StatusName ASC";

            var con = new SqlConnection(_constr);
            var cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@LocationID", ddlLocations.SelectedItem.Value);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;
            cmd.Connection = con;

            try
            {
                con.Open();
                ddlStatuses.DataSource = cmd.ExecuteReader();
                ddlStatuses.DataTextField = "StatusName";
                ddlStatuses.DataValueField = "StatusID";
                ddlStatuses.DataBind();

                //if (ddlStatuses.Items.Count > 1)
                //{
                //    ddlStatuses.Enabled = true;
                //}
                //else
                //{

                //    ddlStatuses.Enabled = false;
                //}

             
            }//try
            catch (Exception ex)
            {
                Msg.Text = "ddlLocationsCode routine Error" + ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

       

        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.Msg2.Text = "";
            this.Msg.Text = "";
            this.txtEndDate.Text = "";
            this.txtStartDate.Text = "";
            this.btnExcel.Visible = false;
            this.ddlLocations.Text = "";
            this.ddlStatuses.Text = "";
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

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            //Exports entire contents of gridview 
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=ExportData1.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            var StringWriter = new System.IO.StringWriter();
            var HtmlTextWriter = new HtmlTextWriter(StringWriter);

            GridView1.AllowPaging = false;
            GridView1.DataSource = ViewState["datasetname"];
            GridView1.DataBind();

            GridView1.RenderControl(HtmlTextWriter);
            Response.Write(StringWriter.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
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
            GridView1.DataSource = ViewState["datasetname"];
            GridView1.DataBind();
            
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
                                                    "WHERE ((CAST(RecordedDate As DATE) >= @From) AND (CAST(RecordedDate As DATE) <= @To) AND (Locations.IsActive=1)) " +
                                                    "AND (@LocationID='' OR LocationID=@LocationID) AND (@StatusID='' OR StatusID=@StatusID) " +
                                                    "GROUP BY Locations.Id, Locations.Name, Locations.Code, Statuses.Name, Statuses.Image, Locations.IsActive, RecordedDate " +
                                                    "ORDER BY RecordedDate DESC, Locations.Name DESC", con))
                    {
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            if (!string.IsNullOrWhiteSpace(txtStartDate.Text) && !string.IsNullOrWhiteSpace(txtEndDate.Text))
                            {
                                //Prevent start date from being greater than end date
                                if (Convert.ToDateTime(this.txtStartDate.Text) > Convert.ToDateTime(this.txtEndDate.Text))
                                {
                                    Msg.Text = "Start date cannot be greater than end date";
                                    return;
                                }
                            }

                            DateTime start;
                            DateTime end;

                            var minDate = DateTime.UtcNow.AddYears(-250);

                            if (string.IsNullOrWhiteSpace(txtStartDate.Text))
                            {
                                start = minDate;
                            }
                            else
                            {
                                start = DateTimeOffset.Parse(txtStartDate.Text).UtcDateTime;

                                if (start < minDate)
                                {
                                    start = minDate;
                                }
                            }

                            if (string.IsNullOrWhiteSpace(txtEndDate.Text))
                            {
                                end = DateTime.MaxValue;
                            }
                            else
                            {
                                end = DateTimeOffset.Parse(txtEndDate.Text).UtcDateTime;

                                if (end < minDate)
                                {
                                    end = minDate;
                                }
                            }

                            var startDate = String.Format("{0:MM/dd/yyyy}", start);
                            var endDate = String.Format("{0:MM/dd/yyyy}", end);

                            // Additional filtering

                            if (ddlLocations.SelectedItem.Value != null)
                            {
                                cmd.Parameters.AddWithValue("@LocationID", ddlLocations.SelectedItem.Value);
                            }
                            

                            if (ddlStatuses.SelectedItem.Value != null)
                            {
                                cmd.Parameters.AddWithValue("@StatusID", ddlStatuses.SelectedItem.Value);
                            }
                           

                            cmd.Parameters.AddWithValue("@From", start);
                            cmd.Parameters.AddWithValue("@To", end);
                            var ds = new DataSet();
                            da.Fill(ds);
                            GridView1.DataSource = ds;
                            ViewState["datasetname"] = ds;
                            GridView1.DataBind();
                            con.Close();


                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                this.Msg.Text = "No Records Found";
                                this.Msg2.Text = "";
                                this.btnExcel.Visible = false;
                            }
                            else
                            {
                                this.Msg2.Text = "Results for " + startDate + " to " + endDate;
                                this.Msg.Text = "";
                                btnExcel.Visible = true;
                            }
                        }
                    }//using
                }
            }
        }
    }
}
