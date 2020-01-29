using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRBugTracker.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]    
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public User CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastModifiedDate { get; set; }

        public string LastModifiedById { get; set; }

        public User LastModifiedBy { get; set; }

        public List<Issue> Issues { get; set; }

        public List<UserProject> UserProjects { get; set; }
    }
}
