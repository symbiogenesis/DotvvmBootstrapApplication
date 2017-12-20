using System.Collections.Generic;

namespace RingDownConsole.Models
{
    public class Filter : DashboardDTO
    {
        public bool RefineSearch { get; set; } = true;

        public string Name { get; set; }

        public string ToDateString { get; set; }

        public string FromDateString { get; set; }

        public IEnumerable<string> Statuses { get; set; }

        public IEnumerable<string> Locations { get; set; }
    }
}
