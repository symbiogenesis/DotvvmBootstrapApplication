using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


using System.Drawing;

namespace RingDownCentralConsole
{
    public partial class Default : System.Web.UI.Page
    {

        string constr = ConfigurationManager.ConnectionStrings["csConsole"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
           if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string direction = string.Empty;

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
                  return (SortDirection)ViewState["SortDirection"];
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
            DataTable table = new DataTable();
            // get the connection
            using (SqlConnection conn = new SqlConnection(constr))
            {
                // write the sql statement to execute
                string sql = "SELECT Locations.Id AS LocationID, Code, Locations.Name AS LocationName, " +
                             "Statuses.Name AS Status, Image, Locations.IsActive, Statuses.IsActive, RecordedDate " +
                             "FROM Statuses INNER JOIN(Locations INNER JOIN LocationStatuses ON Locations.Id = LocationStatuses.LocationId) " +
                             "ON Statuses.Id = LocationStatuses.StatusId WHERE(((Locations.IsActive) = 1) AND((Statuses.IsActive) = 1)) " +
                             "ORDER BY RecordedDate DESC, Locations.Name DESC";

                // instantiate the command object to fire
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // get the adapter object and attach the command object to it
                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                    {
                        // fire Fill method to fetch the data and fill into DataTable
                        ad.Fill(table);
                    }
                }
            }
            return table;

        }

        




    }       
}