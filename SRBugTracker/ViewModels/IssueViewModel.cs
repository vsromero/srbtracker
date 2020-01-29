using SRBugTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SRBugTracker.ViewModels
{
    public class IssueViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Severity { get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public int Project { get; set; }

        public List<Project> Projects { get; set; }
    }
}
