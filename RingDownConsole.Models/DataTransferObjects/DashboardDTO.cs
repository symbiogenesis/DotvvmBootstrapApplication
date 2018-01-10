using System;

namespace RingDownConsole.Models
{
    public class DashboardDTO
    {
        public string RecordId { get; set; }

        public string CurrentPhoneUser { get; set; }

        public DateTime RecordedDate { get; set; }
        public string LocationCode { get; set; }
    }
}
