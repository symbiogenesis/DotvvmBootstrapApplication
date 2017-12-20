using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RingDownConsole.Interfaces;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using TrackableEntities.Common.Core;

namespace RingDownConsole.Models
{
    public class User : IdentityUser<Guid>, IAdminLookup, ITrackable, IMergeable
    {
        public User() : base()
        {
        }

        public User(string userName) : base(userName)
        {
        }

        [NotMapped]
        public bool IsAdmin { get; set; }

        [NotMapped]
        Guid IIdentifiable.Id { get => base.Id; set => value = base.Id; }

        [NotMapped]
        public string Name { get => base.UserName; set => value = base.UserName; }

        [NotMapped]
        public bool IsSaved { get; set; } = true;

        [NotMapped]
        [JsonIgnore]
        public bool IsDefault { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string Comments { get; set; }

        [NotMapped]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }

        [NotMapped]
        public Guid EntityIdentifier { get; set; }
    }
}
