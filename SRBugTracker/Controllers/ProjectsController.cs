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
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ProjectsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var currentuserid = _userManager.GetUserId(User);
            var projects = await _context.UserProject
                            .Where(up => up.UserId.Equals(currentuserid))
                            .Select(up => up.Project)
                            .Include(p => p.CreatedBy)
                            .Include(p => p.LastModifiedBy)
                            .ToListAsync();
            return View(projects);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.Id == id);
            var viewmodel = new ProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Members = await GetProjectUsers(id),
                Issues = await GetProjectIssues(id),
            };
            if (project == null)
            {
                return NotFound();
            }

            return View(viewmodel);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                //Create new project object to add.
                Project project = new Project()
                {
                    Name = createViewModel.Name,
                    Description = createViewModel.Description,
                    UserProjects = new List<UserProject>(),
                };
                //From posted email list, get emails that exist in db, then return a new UserProject object list (EqualityComparer is not used here).
                var toaddlist = createViewModel.Members
                    .Where(email => UserExists(email))
                    .Select(email => new UserProject()
                    {
                        Project = project,
                        User = _context.Users.FirstOrDefault(u => u.Email.Equals(email)),
                    });

                project.UserProjects.AddRange(toaddlist);
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = project.Id });
            }
            return View(createViewModel);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            var viewmodel = new ProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Members = await GetProjectUsers(id),
            };
            if (project == null)
            {
                return NotFound();
            }
            return View(viewmodel);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Members")] CreateProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Get project entity
                    var project = _context.Project
                        .Include(p => p.UserProjects)
                        .FirstOrDefault(p => p.Id == id);
                    //From posted email list, get emails that exist in db, then return a new UserProject object list (EqualityComparer uses IDs to check equality; needed for the Except method).
                    var updatelist = viewModel.Members
                        .Where(email => UserExists(email))
                        .Select(email => new UserProject()
                        {
                            ProjectId = project.Id,
                            UserId = _context.Users.FirstOrDefault(u => u.Email.Equals(email)).Id,
                        }).ToList();
                    //Get differences between existing list and the posted list. Create list of emails to remove and list of emails to add.
                    var toremove = project.UserProjects.Except(updatelist).ToList();
                    var toadd = updatelist.Except(project.UserProjects).ToList();

                    project.Name = viewModel.Name;
                    project.Description = viewModel.Description;
                    project.UserProjects.RemoveAll(up => toremove.Contains(up));
                    project.UserProjects.AddRange(toadd);                    
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View();
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string email)
        {
            return _context.Users.Any(u => u.Email.Equals(email));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(p => p.Id == id);
        }

        private async Task<List<Project>> GetUserProjectList()
        {
            
            var currentuserid = _userManager.GetUserId(User);
            var projectlist = await _context.UserProject
                .Where(up => up.UserId.Equals(currentuserid))
                .Select(up => up.Project)
                .Include(p => p.CreatedBy)
                .Include(p => p.LastModifiedBy)
                .ToListAsync();
            return projectlist;
        }

        private async Task<List<User>> GetProjectUsers(int? projectid)
        {
            var userlist = await _context.UserProject
                .Where(up => up.ProjectId == projectid)
                .Select(up => up.User)
                .ToListAsync();
            return userlist;
        }

        private async Task<List<Issue>> GetProjectIssues(int? projectid)
        {
            var issuelist = await _context.Issue
                .Where(issue => issue.Project.Id == projectid)
                .ToListAsync();
            return issuelist;
        }
        
    }
}
