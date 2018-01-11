using System;
using System.ComponentModel.DataAnnotations.Schema;
using RingDownConsole.Interfaces;

namespace RingDownConsole.Models
{
    public class LocationStatus : IIdentifiable
    {
        public int Id { get; set; }

        public int LocationId { get; set; }

        [ForeignKey(nameof(LocationId))]
        public virtual Location Location { get; set; }

        public int StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }

        public string CurrentPhoneUser { get; set; }

        public DateTime RecordedDate { get; set; }
    }
}
