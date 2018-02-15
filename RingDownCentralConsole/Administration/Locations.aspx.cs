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
            //If authenicated and role admin
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
            const string strQuery = "SELECT * from Locations Where IsActive=1";
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
            const string strQuery = "SELECT * from Locations Where IsActive=1";
            var cmd = new SqlCommand(strQuery);
            GridView1.DataSource = GetData(cmd);
            GridView1.DataBind();
        }

        private DataTable GetData(SqlCommand cmd)
        {
            cmd.CommandType = CommandType.Text;

            using (var dt = new DataTable())
            {
                using (var con = new SqlConnection(_constr))
                {
                    using (var sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        con.Open();
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                    con.Close();
                    return dt;
                }
            }
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
                    ClearMessage();

                    SetNormalMode();
                    GridView1.DataSource = GetData(cmd);
                    GridView1.DataBind();
                    BindData();
                    con.Close();
                }
                catch (Exception ex)
                {
                    /*Handle error*/
                    ShowMessage($"Connection Error in InactivateLocation module {ex}");
                }
            }
        }

        private void ClearMessage()
        {
            Msg.Text = "";
            Msg.Visible = false;
        }

        private void ShowMessage(string message)
        {
            Msg.Text = message;
            Msg.Visible = true;
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            BindData();
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }

        protected void EditLocation(object sender, GridViewEditEventArgs e)
        {
            ClearMessage();
            SetEditMode(e.NewEditIndex);
            BindData();
        }

        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            SetNormalMode();
            BindData();
        }

        private void SetEditMode(int index)
        {
            GridView1.EditIndex = index;
            GridView1.ShowFooter = false;
        }

        private void SetNormalMode()
        {
            ClearMessage();
            GridView1.EditIndex = -1;
            GridView1.ShowFooter = true;
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
                    ClearMessage();

                    con.Open();

                    var cmd1 = new SqlCommand("Select Count(*) from Locations where Id!=@Id and SerialNumber=@SerialNumber", con);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                    cmd1.Parameters.Add("@SerialNumber", SqlDbType.NVarChar).Value = SerialNumber;
                    var existing = cmd1.ExecuteScalar();
                    var numOfExisting = existing != null ? Convert.ToInt32(existing) : 0;

                    if (numOfExisting > 0)
                    {
                        //Serial Exists
                        ShowMessage("Location/Serial Numbers exists in database. Please review active and unactive location records");
                    }
                    else
                    {
                        var cmd2 = new SqlCommand("Update Locations set Code=@Code, SerialNumber=@SerialNumber, Name=@Name where Id=@Id", con);
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                        cmd2.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                        cmd2.Parameters.Add("@Code", SqlDbType.NVarChar).Value = Code;
                        cmd2.Parameters.Add("@SerialNumber", SqlDbType.NVarChar).Value = SerialNumber;
                        var updated = cmd2.ExecuteNonQuery();

                        if (updated > 0)
                        {
                            var cmd3 = new SqlCommand("Select* From Locations WHERE IsActive = 1", con);
                            SetNormalMode();
                            GridView1.DataSource = GetData(cmd3);
                            GridView1.DataBind();
                            con.Close();
                        }
                        else
                        {
                            ShowMessage("Update failed. No rows updated.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    /*Handle error*/
                    ShowMessage($"Connection Error in UpdateLocation module {ex}");
                }
            }
        }

        private string GetExistingSerialNumber(SqlCommand cmd)
        {
            SqlDataReader reader = cmd.ExecuteReader();

            string existingSerialNumber = null;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    existingSerialNumber = reader.GetString(1);
                }
            }
            else
            {
                ShowMessage("Location record not found. Cannot update this record.");
            }

            reader.Close();

            return existingSerialNumber;
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

                    if (reader.HasRows)
                    {
                        //Serial Exists
                        ShowMessage("Location/Serial Numbers exists in database. Please review active and unactive location records");
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
                    ShowMessage($"Connection Error in AddNewLocation module {ex}");
                }
            }
        }
    }
}