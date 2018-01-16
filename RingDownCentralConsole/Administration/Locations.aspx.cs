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
                string Id = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblId")).Text;
                string Name = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtName")).Text;
                string Code = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtCode")).Text;

                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update Locations set Code=@Code, " +
                     "Name=@Name where Id=@Id;Select * From Locations WHERE IsActive=1";

                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                    cmd.Parameters.Add("@Code", SqlDbType.NVarChar).Value = Code;

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
                string Code = ((TextBox)GridView1.FooterRow.FindControl("txtCode")).Text;
                string Name = ((TextBox)GridView1.FooterRow.FindControl("txtName")).Text;

                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    //wow made the mistake of leaving out the station id field in select statement (drove me crazzzzy!)
                    cmd.CommandText = "Insert into Locations (Code, Name, IsActive) " +
                    "values (@Code, @Name, @IsActive);" +
                    "Select * From Locations WHERE IsActive=1";
                    cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = Code;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
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