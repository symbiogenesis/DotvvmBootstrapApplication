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
    public partial class Locations : System.Web.UI.Page
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
            string strQuery = "SELECT * from Locations Where IsActive=1";
            //  SqlCommand cmd = new SqlCommand(strQuery);
            var cmd = new SqlCommand(strQuery);
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

               
        protected void InactivateLocation(object sender, EventArgs e)
        {         
            
            using (SqlConnection con = new SqlConnection(constr))
            {
                LinkButton lnkRemove = (LinkButton)sender;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update Locations set IsActive=@IsActive Where Id=@Id;" +
                     "Select * from Locations Where IsActive=1";
                    cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = lnkRemove.CommandArgument;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 0;

                    GridView1.EditIndex = -1;
                    GridView1.DataSource = GetData(cmd);
                    GridView1.DataBind();
                    BindData();
                }
                catch (Exception ex)
                {
                    /*Handle error*/
                    Msg.Text = "Connection Error in InactivateLocation module" + ex;
                }

            }
            
           
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            BindData();
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }


        protected void EditLocation(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindData();
        }

        protected void UpdateLocation(object sender, GridViewUpdateEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                string Id = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblLocationId")).Text;
                string LoationName = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtLocationName")).Text;
                string Code = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("Code")).Text;

                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update Locations set Code=@Code, " +
                     "LocationName=@LocationName where Id=@Id;Select * From Locations WHERE IsActive=1";

                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                    cmd.Parameters.Add("@LocationName", SqlDbType.VarChar).Value = LocationName;
                    cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = Code;

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
              

        protected void AddNewLocation(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                string Code = ((TextBox)GridView1.FooterRow.FindControl("txtLocationCode")).Text;
                string LocationName = ((TextBox)GridView1.FooterRow.FindControl("txtLocationName")).Text;

                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    //wow made the mistake of leaving out the station id field in select statement (drove me crazzzzy!)
                    cmd.CommandText = "Insert into tblStations (Code, LocationName, IsActive) " +
                    "values (@Code, @LocationName, @IsActive);" +
                    "Select * From Locations WHERE IsActive=1";
                    cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = Code;
                    cmd.Parameters.Add("@LocationName", SqlDbType.VarChar).Value = LocationName;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 1;

                    GridView1.DataSource = GetData(cmd);
                    GridView1.DataBind();
                    BindData();
                }
                catch (Exception ex)
                {
                    /*Handle error*/
                    Msg.Text = "Connection Error in AddNewLocation module" + ex;
                }

            }


        }



       
    }
}