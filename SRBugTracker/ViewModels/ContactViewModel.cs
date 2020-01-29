using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SRBugTracker.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [Display(Name = "Message Body")]
        public string MessageBody { get; set; }

        public string UserEmail { get; set; }
    }
}
