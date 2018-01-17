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
            if (FileUpload1.PostedFile != null && FileUpload1.PostedFile.ContentLength > 0)
            {
             
                string fileName = FileUpload1.FileName;
                string savePath = Server.MapPath(string.Format("~/Images/", fileName));
                string pathToCheck = savePath + fileName;
                string extension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                string Name = txtName.Text.Trim();
       
                try
                {
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
                    {


                        if (System.IO.File.Exists(pathToCheck))
                        {
                            // Notify the user that the file name was changed.
                            lblResult.Text = "An image with this name already exists";
                            return;
                        }
                        else
                        {  

                        // Append the name of the file to upload to the path.
                        savePath += fileName;

                        // file to the specified directory.
                        FileUpload1.SaveAs(savePath);
                            
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into Statuses (Name, ImageName, Image, IsActive) " +
                        "values (@Name, @ImageName, @Image, @IsActive);" +
                        "Select * from Statuses Where IsActive=1";

                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                        cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = fileName;
                        cmd.Parameters.Add("@Image", SqlDbType.NVarChar).Value = "Images/" + fileName;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 1;
                        GridView1.DataSource = GetData(cmd);
                        GridView1.DataBind();

                        // Notify the user that the file was saved successfully.
                        lblResult.Text = "The record has been inserted.";
                                             
                        txtName.Text = string.Empty;

                        }
                    }
                    else
                    {
                        lblResult.Text = "Only images excepted (.jpg, .png or .bmp)";
                        return;
                    }

                                                 
                }
                catch (Exception ex)
                {
                    lblResult.Text = "upload_Click error" + ex;
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
             //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Your Message');", true);
            FileUpload FileUpload2 = GridView1.Rows[e.RowIndex].FindControl("FileUpload2") as FileUpload;

            if (FileUpload2.PostedFile != null && FileUpload2.PostedFile.ContentLength > 0)
            {              
                string fileName = FileUpload2.FileName;
                string savePath = Server.MapPath(string.Format("~/Images/", fileName));
                string pathToCheck = savePath + fileName;
                string extension = System.IO.Path.GetExtension(FileUpload2.FileName).ToLower();            
                string Name = ((TextBox) GridView1.Rows[e.RowIndex].FindControl("txtName")).Text.Trim();
                string Id = ((Label) GridView1.Rows[e.RowIndex].FindControl("lblId")).Text;               

                try
                {
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
                    {

                        if (System.IO.File.Exists(pathToCheck))
                        {
                            // Notify the user that the file name was changed.
                            Msg.Text = "An image with this name already exists";
                            return;
                        }
                        else
                        {  

                        // Append the name of the file to upload to the path.
                        savePath += fileName;

                        // file to the specified directory.
                        FileUpload2.SaveAs(savePath);

                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Update Statuses set Image=@Image, " +
                     "ImageName=@ImageName, Name=@Name where Id=@Id;Select * From Statuses WHERE IsActive=1";

                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                        cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = fileName;
                        cmd.Parameters.Add("@Image", SqlDbType.NVarChar).Value = "Images/" + fileName;
                        GridView1.EditIndex = -1;
                        GridView1.DataSource = GetData(cmd);
                        GridView1.DataBind();

                        // Notify the user that the file was saved successfully.
                        lblResult.Text = "The record has been updated.";

                            // txtName.Text = string.Empty;            
                        }

                    }
                    else
                    {
                        Msg.Text = "Only image files are allowed";
                    }
                }
                catch (Exception ex)
                {
                    Msg.Text = "UpdateStatus Error " + ex;
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
