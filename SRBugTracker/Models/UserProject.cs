using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRBugTracker.Models
{
    public class UserProject: IEquatable<UserProject>
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public bool Equals(UserProject other)
        {
            if (other is null)
            {
                return false;
            }

            return this.UserId == other.UserId && this.ProjectId == other.ProjectId;
        }

        public override bool Equals(object obj) => Equals(obj as UserProject);
        public override int GetHashCode() => (UserId, ProjectId).GetHashCode();
    }
}
