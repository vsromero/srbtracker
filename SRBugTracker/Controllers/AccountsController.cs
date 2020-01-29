using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SRBugTracker.Data;
using SRBugTracker.Models;
using SRBugTracker.ViewModels;

namespace SRBugTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        
        public AccountsController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> userlist = new List<UserViewModel>();
            foreach (var user in _userManager.Users.ToList())
            {
                UserViewModel model = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    UserRole = string.Join(", ", await _userManager.GetRolesAsync(user))
                };
                userlist.Add(model);              
            }
            return View(userlist);
        }

        // GET: Accounts/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserViewModel userview = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                UserRole = string.Join(", ", await _userManager.GetRolesAsync(user))
            };            
            return View(userview);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            UserViewModel model = new UserViewModel();
            model.UserRoles = _roleManager.Roles.ToList();
            return View(model);
        }

        // POST: Accounts/Create
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel userview)
        {
            User user = new User
            {
                FirstName = userview.FirstName,
                LastName = userview.LastName,
                UserName = userview.Email,
                Email = userview.Email,
                PhoneNumber = userview.PhoneNumber,                
            };           

            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, userview.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userview.UserRole));
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(userview);
        }

        //GET: Accounts/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (id.Equals(""))
            {
                return NotFound();
            }
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserViewModel userview = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                UserRole = string.Join(", ", await _userManager.GetRolesAsync(user)),
                UserRoles = _roleManager.Roles.ToList(),
            };
            return View(userview);
        }

        //POST: Accounts/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel userview)
        {
            if (id.Equals(""))
            {
                return NotFound();
            }
            User user = await _userManager.FindByIdAsync(userview.Id);
            user.FirstName = userview.FirstName;
            user.LastName = userview.LastName;
            user.Email = userview.Email;
            user.PhoneNumber = userview.PhoneNumber;
            userview.UserRoles = _roleManager.Roles.ToList();
            try
            {
                var result = await _userManager.UpdateAsync(user);
                await _userManager.RemoveClaimsAsync(user, await _userManager.GetClaimsAsync(user));
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userview.UserRole));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(userview);
        }

        //GET: Accounts/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (id.Equals(""))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        //POST: Accounts/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}