﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotvvmBootstrapApplication.Models
{
    public class Audit : INotifyPropertyChanged
    {
        [Key]
        public Guid Id { get; set; }
        public string ChangeType { get; set; }
        public string ObjectType { get; set; }
        public string FromJson { get; set; }
        public string ToJson { get; set; }
        public DateTime DateCreated { get; set; }
        [ForeignKey(nameof(AuditUser))]
        public Guid AuditUserId { get; set; }
        public string IpAddress { get; set; }
        public string TableName { get; set; }
        public string IdentityJson { get; set; }

        public User AuditUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { }
            remove { }
        }
    }
}
