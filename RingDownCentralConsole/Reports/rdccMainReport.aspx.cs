using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace RingDownCentralConsole.Reports
{
    public partial class rdccMainReport : Page
        {
            protected void Page_Load(object sender, EventArgs e)
            {
            if ((User.Identity.IsAuthenticated) && (User.IsInRole("Administrator")))
            {
                if (!IsPostBack)
                {


                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report.rdlc");
                    Console dsConsole = GetData("SELECT Locations.Id AS LocationID, Code, Locations.Name AS LocationName, " +
                                 "Statuses.Name AS Status, Image, Locations.IsActive, Statuses.IsActive, RecordedDate " +
                                 "FROM Statuses INNER JOIN(Locations INNER JOIN LocationStatuses ON Locations.Id = LocationStatuses.LocationId) " +
                                 "ON Statuses.Id = LocationStatuses.StatusId WHERE(((Locations.IsActive) = 1) AND((Statuses.IsActive) = 1)) " +
                                 "ORDER BY RecordedDate DESC, Locations.Name DESC");

                    var datasource = new ReportDataSource("Console", dsConsole.Tables[0]);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                }
            }
            else
            {
                //Send user back to main console page, because user is not an "Administrator" role
                Response.Redirect("~/Account/Login.aspx");
            }
            }

            private Console GetData(string query)
            {
                var conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var cmd = new SqlCommand(query);
                using (var con = new SqlConnection(conString))
                {
                    using (var sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;

                        sda.SelectCommand = cmd;
                        using (var dsConsole = new Console())
                        {
                            sda.Fill(dsConsole, "DataTable1");
                            return dsConsole;
                        }
                    }
                }
            }
        }
    }
