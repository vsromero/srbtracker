using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SRBugTracker.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Body { get; set; }

        public int IssueId { get; set; }

        public Issue Issue { get; set; }

        public string CreatedById { get; set; }

        public User CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public string LastModifiedById { get; set; }

        public User LastModifiedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastModifiedDate { get; set; }
    }
}
