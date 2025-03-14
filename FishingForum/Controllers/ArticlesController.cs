using FishingForum.Services.Interfaces;
using FishingForum.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FishingForum.Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IUserService _userService;

        public ArticlesController(IArticleService articleService, IUserService userService)
        {
            _articleService = articleService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticlesWithUsersAsync();
            return View(articles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = await _articleService.GetArticleWithUserAndCommentsByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return Unauthorized();
                }

                article.UserId = userId;
                article.CreatedAt = DateTime.UtcNow;

                await _articleService.CreateArticleAsync(article);
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleService.GetArticleWithUserByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _articleService.UpdateArticleAsync(article);
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _articleService.GetArticleWithUserByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _articleService.DeleteArticleAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
