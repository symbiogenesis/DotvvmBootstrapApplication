using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotvvmBootstrapApplication.Interfaces;

namespace DotvvmBootstrapApplication.Models
{
    public class ExampleRecord : IIdentifiable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string RecordNum { get; set; }

        public string Comments { get; set; }

        public DateTime LoggedDateTime { get; set; }
        
    }
}
