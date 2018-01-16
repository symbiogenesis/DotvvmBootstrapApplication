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

namespace RingDownCentralConsole
{
    public partial class Statuses : System.Web.UI.Page
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
            string strQuery = "SELECT * from Statuses Where IsActive=1";
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

        protected void upload_Click(object sender, EventArgs e)
        {
            try
            {

                // Before attempting to save the file, verify
                // that the FileUpload control contains a file.
                if (FileUpload1.HasFile)
                {
                    //Since there is a file, does the file have the appropriate extension?
                    string extension = System.IO.Path.GetExtension(FileUpload1.FileName);

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
                    {
                        // Call a helper method routine to save the file.
                        SaveFile(FileUpload1.PostedFile);

                    }
                    else
                    {
                        lblResult.Text = "Only files with .jpg, .png or .bmp extensions are allowed.";
                    }

                }


                else
                {
                    // Notify the user that a file was not uploaded.
                    lblResult.Text = "You did not specify a file to upload.";

                }
            }
            catch (Exception ex)
            {
                upload.Text = ex.Message;
            }


        }
        
        void SaveFile(HttpPostedFile file)
        {
            // Specify the path to save the uploaded file to.
            //  string savePath = "~/Images/";

            // Get the name of the file to upload.
            string fileName = FileUpload1.FileName;

            string savePath = Server.MapPath(string.Format("~/Images/", fileName));

            // Create the path and file name to check for duplicates.
            string pathToCheck = savePath + fileName;

            // Check to see if a file already exists with the
            // same name as the file to upload.        
            if (System.IO.File.Exists(pathToCheck))
            {
                // Notify the user that the file name was changed.
                lblResult.Text = "A file with the same name already exists.";

            }
            else
            {

                // Append the name of the file to upload to the path.
                savePath += fileName;

                // Call the SaveAs method to save the uploaded
                // file to the specified directory.
                FileUpload1.SaveAs(savePath);

                using (SqlConnection con = new SqlConnection(constr))
                {
                    string Name = txtName.Text.Trim();

                    try
                    {

                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;

                        cmd.CommandText = "Insert into Statuses (Name, ImageName, Image, IsActive) " +
                        "values (@Name, @ImageName, @Image, @IsActive);" +
                        "Select * from tblStatuses Where IsActive=1";
                        // cmd.Parameters.Add("@StatusCode", SqlDbType.Int).Value = StatusCode;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                        cmd.Parameters.Add("@ImageName", SqlDbType.VarChar).Value = fileName;
                        cmd.Parameters.Add("@Image", SqlDbType.NVarChar).Value = "Images/" + fileName;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 1;
                        GridView1.DataSource = GetData(cmd);
                        GridView1.DataBind();
                        // Notify the user that the file was saved successfully.
                        lblResult.Text = "The record has been inserted.";

                        // txtStatusCode.Text = string.Empty;
                        txtName.Text = string.Empty;

                    }
                    catch (Exception ex)
                    {
                        /*Handle error*/
                        Msg.Text = "Connection Error in Upload_Click module" + ex;
                    }

                }
            }
        }

        protected void InactivateRecord(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                LinkButton lnkRemove = (LinkButton) sender;

                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "Update Statuses set IsActive=@IsActive Where Id=@Id;" +
                     "Select * from Statuses Where IsActive=1";
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = lnkRemove.CommandArgument;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 0;

                    GridView1.DataSource = GetData(cmd);
                    GridView1.DataBind();
                    BindData();

                }
                catch (Exception ex)
                {
                    /*Handle error*/
                    Msg.Text = "Connection Error in InactivateRecord module" + ex;
                }

            }

        }





        protected void UpdateStatus(object sender, GridViewUpdateEventArgs e)
        {









            using (SqlConnection con = new SqlConnection(constr))
            {
                string Id = ((Label) GridView1.Rows[e.RowIndex].FindControl("lblId")).Text;
                string Name = ((TextBox) GridView1.Rows[e.RowIndex].FindControl("txtName")).Text;
                string Code = ((TextBox) GridView1.Rows[e.RowIndex].FindControl("txtCode")).Text;

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
