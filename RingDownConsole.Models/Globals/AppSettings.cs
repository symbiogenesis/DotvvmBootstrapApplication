namespace RingDownConsole.Models
{
    public class AppSettings
    {
        public string ApplicationName { get; set; }
        public string Version { get; set; }
        public int PageSize { get; set; }
        public string SiteUri { get; set; }
        public string RingDownConsoleDb { get; set; }
        public int MaxAgeForDashboardLogs { get; set; }
    }
}
