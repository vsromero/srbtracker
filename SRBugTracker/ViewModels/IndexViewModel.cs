using SRBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRBugTracker.ViewModels
{
    public class IndexViewModel
    {
        public int OpenIssuesCount { get; set; }
        public int ClosedIssuesCount { get; set; }
        public IEnumerable<Issue> LastWeekOpenedIssues { get; set; }
        public IEnumerable<Issue> LastWeekClosedIssues { get; set; }
        public decimal AverageOpen { get; set; }
        public decimal AverageClosed { get; set; }
        public IEnumerable<Issue> RecentModified { get; set; }
        public IEnumerable<Issue> RecentCreated { get; set; }
    }
}
