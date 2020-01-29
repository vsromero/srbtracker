using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRBugTracker.Data;
using SRBugTracker.Models;
using SRBugTracker.ViewModels;
using HtmlAgilityPack;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace SRBugTracker.Controllers
{    
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                bool isClosed(Status status)
                {
                    var closedStatusList = new List<Status> { Status.Closed, Status.Fixed, Status.Verified };
                    return closedStatusList.Contains(status);
                }

                var currentuserid = _userManager.GetUserId(User);
                var issues = _context.UserProject
                    .Where(up => up.UserId.Equals(currentuserid))
                    .SelectMany(up => up.Project.Issues)
                    .Include(issue => issue.CreatedBy)
                    .Include(issue => issue.LastModifiedBy)
                    .Include(issue => issue.Project)
                    .ToList();

                var openIssues = issues.Where(i => !isClosed(i.Status));
                var closedIssues = issues.Where(i => isClosed(i.Status));

                var issuesOpenedLastWeek = openIssues.Where(i => i.CreatedDate >= DateTime.UtcNow.AddDays(-7));
                var issuesClosedLastWeek = closedIssues.Where(i => i.LastModifiedDate >= DateTime.UtcNow.AddDays(-7));

                var firstCreated = issues.Min(i => i.CreatedDate);
                var lastCreated = issues.Max(i => i.CreatedDate);
                var weeks = lastCreated.Subtract(firstCreated).Days / 7;

                var averageOpenedWeek = Math.Round((decimal) issues.Count / weeks, 1);
                var averageClosedWeek = Math.Round((decimal) issues.Where(i => isClosed(i.Status)).Count() / weeks, 1);

                var recentModified = issues.OrderByDescending(i => i.LastModifiedDate).Take(5);
                var recentCreated = issues.OrderByDescending(i => i.CreatedDate).Take(5);

                IndexViewModel indexView = new IndexViewModel()
                {
                    OpenIssuesCount = openIssues.Count(),
                    ClosedIssuesCount = closedIssues.Count(),
                    LastWeekOpenedIssues = issuesOpenedLastWeek,
                    LastWeekClosedIssues = issuesClosedLastWeek,
                    AverageOpen = averageOpenedWeek,
                    AverageClosed = averageClosedWeek,
                    RecentModified = recentModified,
                    RecentCreated = recentCreated,
                };

                return View(indexView);                
            }
            return View("LandingPage");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel viewModel)
        {            
            return View("Sent");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search(string searchString)
        {
            var currentuserid = _userManager.GetUserId(User);

            var projects = _context.UserProject
                .Where(up => up.UserId.Equals(currentuserid))
                .Select(up => up.Project);
            var projectResults = projects
                .Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString))
                .Select(p => new SearchResultViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = RemoveHtmlTags(p.Description),
                    Type = "Projects",
                });

            var issues = projects.SelectMany(p => p.Issues);
            var issueResults = issues
                .Where(i => i.Name.Contains(searchString) || i.Description.Contains(searchString))
                .Select(i => new SearchResultViewModel()
                {                    
                    Id = i.Id,
                    Name = i.Name,
                    Description = RemoveHtmlTags(i.Description),
                    Type = "Issues",
                });
            
            var results = issueResults.Concat(projectResults).ToList();
            return View(results);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string RemoveHtmlTags(string s)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(s);
            return htmlDoc.DocumentNode.InnerText;
        }
    }
}
