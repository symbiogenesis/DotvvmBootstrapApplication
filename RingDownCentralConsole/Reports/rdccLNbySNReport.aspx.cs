using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Configuration;

namespace RingDownCentralConsole.Reports
{
    
        public partial class rdccLNbySNReport : System.Web.UI.Page
        {
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {


                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rdccLSCount.rdlc");
                    Console2 dsConsole2 = GetData("SELECT LocName, StatusName, Count(StatusName) AS [Count] " +
                                                "FROM (tblConsole INNER JOIN tblStations ON tblConsole.StationID = tblStations.StationID) " +
                                                "INNER JOIN tblStatuses ON tblConsole.StatusID = tblStatuses.StatusID " +
                                                "WHERE (((tblStatuses.IsActive)=1) AND ((tblStations.IsActive)=1)) " +
                                                "GROUP BY LocName, StatusName " +
                                                "ORDER BY LocName, StatusName");

                    ReportDataSource datasource = new ReportDataSource("Console2", dsConsole2.Tables[0]);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                }
            }

            private Console2 GetData(string query)
            {
                string conString = ConfigurationManager.ConnectionStrings["csConsole"].ConnectionString;
                SqlCommand cmd = new SqlCommand(query);
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;

                        sda.SelectCommand = cmd;
                        using (Console2 dsConsole2 = new Console2())
                        {
                            sda.Fill(dsConsole2, "DataTable1");
                            return dsConsole2;
                        }
                    }
                }
            }
        }
    }
