using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using RingDownConsole.Interfaces;

namespace RingDownConsole.Models
{
    public class User : IdentityUser<int>, IAdminLookup
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
        int IIdentifiable.Id { get => base.Id; set => value = base.Id; }

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
    }
}
