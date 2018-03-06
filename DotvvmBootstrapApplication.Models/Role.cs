using System;
using System.ComponentModel.DataAnnotations.Schema;
using DotvvmBootstrapApplication.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DotvvmBootstrapApplication.Models
{
    public class Role : IdentityRole<Guid>, IIdentifiable
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
    }
}
