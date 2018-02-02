using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace RingDownCentralConsole.Administration
{

   

    public partial class SetRefresh : System.Web.UI.Page
    {
        private readonly string _path = "~/Interval/RefreshVal.json";

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((User.Identity.IsAuthenticated) && (User.IsInRole("Administrator")))
            {
                BindData();
             
            }
            else
            {
                //Send user back to main console page, because user is not an "Administrator" role
                Response.Redirect("~/Account/Login.aspx");
            }
        }


        private void BindData()
        {

           
            try
            {
                using (var sr = new StreamReader(Server.MapPath(_path)))
                {
                    txtRefreshVal.Text = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Msg.Text = "Page Load Error:" + ex.ToString();
            }


        }

        protected void SetInterval_Click(object sender, EventArgs e)
        {
            //var sb = new StringBuilder();
            //var sw = new StringWriter(sb);

            var seconds = ddlIntervalSeconds.SelectedValue.ToString();          

            //Update .json file with value, replace each value
            using (var _val = new StreamWriter(Server.MapPath(_path), false))
            {
                try
                {
                    //_testData.Write(seconds); // Write the file.
                    _val.WriteLine(seconds);
                    _val.Flush();
                    _val.Close();
                    _val.Dispose();

                    Msg.Text = "The Ring Down Central Console refresh interval has been set to " + seconds + " seconds.";

                    BindData();
                  
                }
                catch (IOException ex)
                {
                    Msg.Text = "IOException:\r\n\r\n" + ex.Message;                   
                }
                catch (Exception ex)
                {                   
                    Msg.Text = "Exception:\r\n\r\n" + ex.Message;
                }
                finally
                {
                    if (_val != null)
                    {
                      //  _val.Flush();
                      ////  _val.Close();
                      //  _val.Dispose();
                       
                    }
                }







            }
        }
    }
}