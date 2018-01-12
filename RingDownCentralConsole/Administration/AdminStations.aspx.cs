using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace RingDownCentralConsole
{
    public partial class AdminStations : System.Web.UI.Page
    {
      
        string constr = ConfigurationManager.ConnectionStrings["csConsole"].ToString();
        
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
            string strQuery = "SELECT * from tblStations Where IsActive=1";
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

               
        protected void InactivateStation(object sender, EventArgs e)
        {         
            
            using (SqlConnection con = new SqlConnection(constr))
            {
                LinkButton lnkRemove = (LinkButton)sender;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update tblStations set IsActive=@IsActive Where StationID=@StationID;" +
                     "Select * from tblStations Where IsActive=1";
                    cmd.Parameters.Add("@StationID", SqlDbType.VarChar).Value = lnkRemove.CommandArgument;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 0;

                    GridView1.EditIndex = -1;
                    GridView1.DataSource = GetData(cmd);
                    GridView1.DataBind();
                    BindData();
                }
                catch (Exception ex)
                {
                    /*Handle error*/
                    Msg.Text = "Connection Error in InactivateStation module" + ex;
                }

            }
            
           
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            BindData();
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }


        protected void EditStation(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindData();
        }

        protected void UpdateStation(object sender, GridViewUpdateEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                string StationID = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblStationID")).Text;
                string LocName = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtLocName")).Text;
                string LocID = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtLocID")).Text;

                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update tblStations set LocID=@LocID, " +
                     "LocName=@LocName where StationID=@StationID;Select * From tblStations WHERE IsActive=1";

                    cmd.Parameters.Add("@StationID", SqlDbType.Int).Value = StationID;
                    cmd.Parameters.Add("@LocName", SqlDbType.VarChar).Value = LocName;
                    cmd.Parameters.Add("@LocID", SqlDbType.VarChar).Value = LocID;

                    GridView1.EditIndex = -1;
                    GridView1.DataSource = GetData(cmd);
                    GridView1.DataBind();
                }
                catch (Exception ex)
                {
                    /*Handle error*/
                    Msg.Text = "Connection Error in UpdateStation module" + ex;
                }

            }
            
        }
              

        protected void AddNewStation(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                string LocID = ((TextBox)GridView1.FooterRow.FindControl("txtLocID")).Text;
                string LocName = ((TextBox)GridView1.FooterRow.FindControl("txtLocName")).Text;

                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    //wow made the mistake of leaving out the station id field in select statement (drove me crazzzzy!)
                    cmd.CommandText = "Insert into tblStations (LocID, LocName, IsActive) " +
                    "values (@LocID, @LocName, @IsActive);" +
                    "Select * From tblStations WHERE IsActive=1";
                    cmd.Parameters.Add("@LocID", SqlDbType.VarChar).Value = LocID;
                    cmd.Parameters.Add("@LocName", SqlDbType.VarChar).Value = LocName;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 1;

                    GridView1.DataSource = GetData(cmd);
                    GridView1.DataBind();
                    BindData();
                }
                catch (Exception ex)
                {
                    /*Handle error*/
                    Msg.Text = "Connection Error in AddNewStation module" + ex;
                }

            }


        }



       
    }
}