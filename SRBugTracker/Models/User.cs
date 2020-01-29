using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SRBugTracker.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public override bool EmailConfirmed { get; set; }

        public override string PhoneNumber { get; set; }
        
        public List<UserProject> UserProjects { get; set; }
    }
}
