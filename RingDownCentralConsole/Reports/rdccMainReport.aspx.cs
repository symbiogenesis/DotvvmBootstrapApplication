using Microsoft.Reporting.WebForms;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace RingDownCentralConsole.Reports
{
    public partial class rdccMainReport : Page
        {
            protected void Page_Load(object sender, EventArgs e)
            {
            ////If authenicated and role admin
            //if ((Page.User.Identity.IsAuthenticated) && (Roles.IsUserInRole("Administrator")))
            //{
            if (!IsPostBack)
            {


                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report.rdlc");
                Console dsConsole = GetData("SELECT LocID, LocName, StatusCode, Image, PersonOnline, StatusName " +
                                            "FROM (tblConsole INNER JOIN tblStations ON tblConsole.StationID = tblStations.StationID) " +
                                            "INNER JOIN tblStatuses ON tblConsole.StatusID = tblStatuses.StatusID " +
                                            "WHERE(((tblStatuses.IsActive) = 1) AND((tblStations.IsActive) = 1)) " +
                                            "ORDER BY tblConsole.DteTime, tblStations.LocID");

                ReportDataSource datasource = new ReportDataSource("Console", dsConsole.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);
            }
            //}
            //else
            //{
            //    // Response.Redirect("Login.aspx");
            //}     
        }

            private Console GetData(string query)
            {
                string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlCommand cmd = new SqlCommand(query);
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;

                        sda.SelectCommand = cmd;
                        using (Console dsConsole = new Console())
                        {
                            sda.Fill(dsConsole, "DataTable1");
                            return dsConsole;
                        }
                    }
                }
            }
        }
    }
