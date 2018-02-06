using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace RingDownCentralConsole
{
    public partial class _Default : Page
    {
        private readonly string _constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        //private readonly string _path = "~/Interval/RefreshVal.json";

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((User.Identity.IsAuthenticated) && (User.IsInRole("Administrator")) || (User.IsInRole("User")))
            {
                if (!IsPostBack)
                {
                    BindData();
                }
            }
            else
            {
                //IF NOT User or Administrator, yet Authenticated, then User does not have a role.  Sign user out and direct to
                //error page               
                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Response.Redirect("~/Messages/AccountReview.aspx");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                var row = e.Row;

                var start = DateTime.UtcNow;
                var recordedDate = DataBinder.Eval(row.DataItem, "RecordedDate");
                var recDateTime = (recordedDate == null) ? DateTime.MinValue : Convert.ToDateTime(recordedDate.ToString());

                var minutes = Math.Floor((start - recDateTime).TotalMinutes);

                var statusCell = row.Cells[2];

                //have 10 minutes since
                if (minutes >= 10)
                {
                    var imageCell = row.Cells[3];

                    row.Attributes.CssStyle.Value = "background-color: #EE6363; color: #00000";
                    statusCell.Text = "Disconnected";
                    var image = (Image) imageCell.FindControl("Image1");
                    image.ImageUrl = "~/Images/no_link_t.png";
                    return;
                }

                switch (statusCell.Text)
                {
                    case "No Link":
                        row.Attributes.CssStyle.Value = "background-color: #fb968b; color: #00000";
                        break;
                    case "Connected":
                        row.Attributes.CssStyle.Value = "background-color: #AADD00; color: #00000";
                        break;
                    case "No Dial Tone":
                        row.Attributes.CssStyle.Value = "background-color: #EEE9E9; color: #00000";
                        break;
                    case "On Hook":
                        row.Attributes.CssStyle.Value = "background-color: #E8F1D4; color: #00000";
                        break;
                    case "Off Hook":
                        row.Attributes.CssStyle.Value = "background-color: #CDC9C9; color: #00000";
                        break;
                }
            }
            catch (Exception ex)
            {
                /*Handle error*/
                Msg.Text = "Connection Error in GridView1_RowDataBound" + ex;
            }
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
                var sql = "SELECT Name, Code, SerialNumber, IsActive" +
                            "FROM dbo.Locations AS loc" +
                            "CROSS APPLY" +
                            "     (select top 1 * " +
                            "      FROM LocationStatuses" +
                            "      WHERE LocationId = loc.Id" +
                            "      ORDER BY RecordedDate desc) AS ls";

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

        //5 second timer 
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            GridView1.DataBind();
            BindData();
        }
    }
}