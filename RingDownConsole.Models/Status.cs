using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RingDownConsole.Interfaces;
using TrackableEntities.Common.Core;

namespace RingDownConsole.Models
{
    public class Status : IIdentifiable, ITrackable, IMergeable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string ImageName { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }

        [NotMapped]
        public Guid EntityIdentifier { get; set; }
    }
}
