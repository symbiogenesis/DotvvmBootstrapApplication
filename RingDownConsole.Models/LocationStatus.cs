using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RingDownConsole.Interfaces;
using TrackableEntities.Common.Core;

namespace RingDownConsole.Models
{
    public class LocationStatus : IIdentifiable, ITrackable, IMergeable
    {
        public int Id { get; set; }

        public string SerialNumber { get; set; }

        [ForeignKey(nameof(SerialNumber))]
        public virtual Location Location { get; set; }

        public int StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }

        public string CurrentPhoneUser { get; set; }

        public DateTime RecordedDate { get; set; }

        [NotMapped]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }

        [NotMapped]
        public Guid EntityIdentifier { get; set; }
    }
}
