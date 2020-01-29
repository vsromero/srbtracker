using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRBugTracker.Data;
using SRBugTracker.Models;

namespace SRBugTracker.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Body, IssueId")] Comment comment)
        {
            try
            {
                // TODO: Add insert logic here
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Issues", new { id = comment.IssueId });
            }
            catch
            {
                return RedirectToAction("Details", "Issues", new { id = comment.IssueId });
            }
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Body")] Comment comment)
        {
            try
            {
                // TODO: Add update logic here
                _context.Update(comment);
                _context.SaveChanges();
                return RedirectToAction("Details", "Issues", new { id = comment.IssueId });
            }
            catch
            {
                return RedirectToAction("Details", "Issues", new { id = comment.IssueId });
            }
        }

        // POST: Comments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var comment = _context.Comment.Find(id);
            var issueid = comment.IssueId;

            try
            {
                // TODO: Add delete logic here
                _context.Comment.Remove(comment);
                _context.SaveChanges();
                return RedirectToAction("Details", "Issues", new { id = issueid });
            }
            catch
            {
                return RedirectToAction("Details", "Issues", new { id = issueid });
            }
        }
    }
}