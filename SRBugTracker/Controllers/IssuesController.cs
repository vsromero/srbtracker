using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SRBugTracker.Data;
using SRBugTracker.Models;
using SRBugTracker.ViewModels;

namespace SRBugTracker.Controllers
{
    public class IssuesController : Controller
    {
        private readonly RDSContext _context;
        private readonly UserManager<User> _userManager;

        public IssuesController(RDSContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Issues
        public async Task<IActionResult> Index()
        {
            var currentuserid = _userManager.GetUserId(User);
            var issues = await _context.UserProject
                .Where(up => up.UserId.Equals(currentuserid))
                .SelectMany(up => up.Project.Issues)
                .Include(issue => issue.CreatedBy)
                .Include(issue => issue.LastModifiedBy)
                .Include(issue => issue.Project)
                .ToListAsync();

            return View(issues);
        }

        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .Include(i => i.Project)
                .Include(i => i.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // GET: Issues/Create
        public IActionResult Create()
        {
            //GET PROJECTS WHERE USER IS IN
            var currentuserid = _userManager.GetUserId(User);
            var projectlist = _context.UserProject
                .Where(up => up.UserId.Equals(currentuserid))
                .Select(up => up.Project)
                .Include(p => p.CreatedBy)
                .Include(p => p.LastModifiedBy)
                .ToList();
            IssueViewModel viewmodel = new IssueViewModel() { Projects = projectlist };
            return View(viewmodel);
        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Severity,Priority,Status,Project")] IssueViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Issue issue = new Issue()
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Severity = viewModel.Severity,
                    Priority = viewModel.Priority,
                    Status = viewModel.Status,
                    Project = _context.Project.Find(viewModel.Project),
                };
                _context.Add(issue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = issue.Id });
            }
            var currentuserid = _userManager.GetUserId(User);
            viewModel.Projects = await _context.UserProject
                .Where(up => up.UserId.Equals(currentuserid))
                .Select(up => up.Project)
                .Include(p => p.CreatedBy)
                .Include(p => p.LastModifiedBy)
                .ToListAsync();
            return View(viewModel);
        }

        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .Include(i => i.Project)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (issue == null)
            {
                return NotFound();
            }

            var viewmodel = new IssueViewModel()
            {
                Id = issue.Id,
                Name = issue.Name,
                Description = issue.Description,
                Priority = issue.Priority,
                Severity = issue.Severity,
                Status = issue.Status,
                Project = issue.Project.Id,
                Projects = _context.Project.ToList(),
            };
            return View(viewmodel);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Severity,Priority,Status,Project")] IssueViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var issue = await _context.Issue.FindAsync(id);
                    issue.Name = viewModel.Name;
                    issue.Description = viewModel.Description;
                    issue.Severity = viewModel.Severity;
                    issue.Priority = viewModel.Priority;
                    issue.Status = viewModel.Status;
                    issue.ProjectId = viewModel.Project;
                    _context.Update(issue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            return View(viewModel);
        }

        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issue = await _context.Issue.FindAsync(id);
            _context.Issue.Remove(issue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueExists(int id)
        {
            return _context.Issue.Any(issue => issue.Id == id);
        }

        public List<Comment> GetComments(int issueid)
        {
            return _context.Comment.Where(comment => comment.IssueId == issueid).ToList();
        }
    }
}
