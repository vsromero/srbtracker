using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRBugTracker.Models
{
    public class Issue
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(1,5)]
        public int Severity { get; set; }

        [Range(1, 5)]
        public int Priority { get; set; }

        [Required]
        public Status Status { get; set; }

        public int ProjectId { get; set; }

        [Required]
        public Project Project { get; set; }

        public List<Comment> Comments { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public User CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastModifiedDate { get; set; }

        public string LastModifiedById { get; set; }

        public User LastModifiedBy { get; set; }

    }

    public enum Status
    {
        New,
        Assigned,
        Open,
        Fixed,
        Testing,
        Verified,
        Reopened,
        Closed,
        Rejected,
        Deferred
    }
}
