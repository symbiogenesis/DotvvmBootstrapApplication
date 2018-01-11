using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using RingDownConsole.Interfaces;

namespace RingDownConsole.Models
{
    public class Role : IdentityRole<int>, IIdentifiable
    {
        public Role() : base()
        {
        }

        public Role(string userName) : base(userName)
        {
        }

        [NotMapped]
        int IIdentifiable.Id { get => base.Id; set => value = base.Id; }

        [NotMapped]
        public bool IsSaved { get; set; } = true;
    }
}
