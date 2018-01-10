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

        public int LocationId { get; set; }

        public string StatusId { get; set; }

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
