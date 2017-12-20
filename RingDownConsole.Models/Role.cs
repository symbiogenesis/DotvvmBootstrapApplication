using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RingDownConsole.Interfaces;
using Microsoft.AspNetCore.Identity;
using TrackableEntities.Common.Core;

namespace RingDownConsole.Models
{
    public class Role : IdentityRole<Guid>, IIdentifiable, ITrackable, IMergeable
    {
        public Role() : base()
        {
        }

        public Role(string userName) : base(userName)
        {
        }

        [NotMapped]
        Guid IIdentifiable.Id { get => base.Id; set => value = base.Id; }

        [NotMapped]
        public bool IsSaved { get; set; } = true;

        [NotMapped]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }

        [NotMapped]
        public Guid EntityIdentifier { get; set; }
    }
}
