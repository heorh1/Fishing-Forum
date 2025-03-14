using FishingForum.DB;
using FishingForum.Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FishingForum.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ByArticle(int articleId, int page = 1)
        {
            int pageSize = 5;
            var comments = await _context.Comments
                .Where(c => c.ArticleId == articleId)
                .OrderBy(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize + 1)
                .Include(c => c.User)
                .ToListAsync();

            bool hasMore = comments.Count > pageSize;
            if (hasMore) comments.RemoveAt(comments.Count - 1);

            ViewBag.ArticleId = articleId;
            ViewBag.HasMore = hasMore;
            ViewBag.NextPage = page + 1;

            return View(comments);
        }

        [HttpGet]
        public IActionResult Create(int articleId)
        {
            return View(new Comment { ArticleId = articleId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                comment.UserId = user.Id;
                comment.CreatedAt = DateTime.Now;
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
            }
            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();
            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
            }
            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();
            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Articles", new { id = comment?.ArticleId });
        }
    }
}
