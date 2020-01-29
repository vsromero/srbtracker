using Microsoft.AspNetCore.Identity;

namespace SRBugTracker.Models
{
    public class Role : IdentityRole
    {
        public Role() : base() { }
        public Role(string name) : base() { }
    }
}
