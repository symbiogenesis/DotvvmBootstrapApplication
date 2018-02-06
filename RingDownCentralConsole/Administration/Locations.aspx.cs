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
    public partial class Locations : Page
    {
        private readonly string _constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

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
            var strQuery = "SELECT * from Locations Where IsActive=1";
            var cmd = new SqlCommand(strQuery);
            GridView1.DataSource = GetData(cmd);
            GridView1.DataBind();
        }

        private DataTable GetData(SqlCommand cmd)
        {
            var dt = new DataTable();
            var con = new SqlConnection(_constr);
            var sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            con.Open();
            sda.SelectCommand = cmd;
            sda.Fill(dt);
            return dt;

        }

        protected void InactivateLocation(object sender, EventArgs e)
        {
            using (var con = new SqlConnection(_constr))
            {
                var lnkRemove = (LinkButton) sender;
                try
                {
                    var cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update Locations set IsActive=@IsActive Where Id=@Id;" +
                     "Select * from Locations Where IsActive=1";
                    cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = lnkRemove.CommandArgument;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 0;
                    Msg.Text = "";

                    GridView1.EditIndex = -1;
                    GridView1.DataSource = GetData(cmd);
                    GridView1.DataBind();
                    BindData();
                    con.Close();
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
            using (var con = new SqlConnection(_constr))
            {
                var Id = ((Label) GridView1.Rows[e.RowIndex].FindControl("lblId")).Text;
                var Name = ((TextBox) GridView1.Rows[e.RowIndex].FindControl("txtName")).Text.Trim();
                var Code = ((TextBox) GridView1.Rows[e.RowIndex].FindControl("txtCode")).Text.Trim();
                var SerialNumber = ((TextBox) GridView1.Rows[e.RowIndex].FindControl("txtSerialNumber")).Text.Trim();

                try
                {

                    con.Open();
                    var cmd = new SqlCommand("Select Id, SerialNumber from Locations where SerialNumber=@SerialNumber", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@SerialNumber", SqlDbType.NVarChar).Value = SerialNumber;
                    SqlDataReader reader = cmd.ExecuteReader();
                    Msg.Text = "";

                    if (reader.HasRows)
                    {
                        //Serial Exists
                        Msg.Text = "Location/Serial Numbers exists in database. Please review active and unactive location records";
                    }
                    else
                    {
                        cmd.CommandText = "Update Locations set Code=@Code, SerialNumber=@SerialNumber, " +
                        "Name=@Name where Id=@Id;Select * From Locations WHERE IsActive=1";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar).Value = Code;
                        // Serial Number defined above
                        GridView1.EditIndex = -1;
                        GridView1.DataSource = GetData(cmd);
                        GridView1.DataBind();
                        con.Close();
                    }

                }
                catch (Exception ex)
                {
                    /*Handle error*/
                    Msg.Text = "Connection Error in UpdateLocation module" + ex;
                }
            }
        }

        protected void AddNewLocation(object sender, EventArgs e)
        {


            using (var con = new SqlConnection(_constr))
            {
                var Code = ((TextBox) GridView1.FooterRow.FindControl("txtCode")).Text.Trim();
                var Name = ((TextBox) GridView1.FooterRow.FindControl("txtName")).Text.Trim();
                var SerialNumber = ((TextBox) GridView1.FooterRow.FindControl("txtSerialNumber")).Text.Trim();

                try
                {
                    con.Open();
                    var cmd = new SqlCommand("Select SerialNumber from Locations where SerialNumber=@SerialNumber", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@SerialNumber", SqlDbType.NVarChar).Value = SerialNumber;
                    SqlDataReader reader = cmd.ExecuteReader();
                    Msg.Text = "";

                    if (reader.HasRows)
                    {
                        //Serial Exists
                        Msg.Text = "Location/Serial Numbers exists in database. Please review active and unactive location records";
                    }
                    else
                    {
                        //Serial does not exist (no duplicate Serial Number)   
                        cmd.CommandText = "Insert into Locations (Code, Name, SerialNumber, IsActive) " +
                      "values (@Code, @Name, @SerialNumber, @IsActive);" +
                       "Select * From Locations WHERE IsActive=1";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar).Value = Code;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                        // SerialNumber declared above
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 1;
                        GridView1.DataSource = GetData(cmd);
                        GridView1.DataBind();
                        con.Close();
                    }
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