using FishingForum.DB;
using FishingForum.Domain.Entities;
using FishingForum.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ArticleService : IArticleService
{
    private readonly ApplicationDbContext _context;

    public ArticleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Article>> GetAllArticlesAsync()
    {
        return await _context.Articles.ToListAsync();
    }

    public async Task<Article> GetArticleByIdAsync(int id)
    {
        return await _context.Articles.FindAsync(id);
    }

    public async Task CreateArticleAsync(Article article)
    {
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateArticleAsync(Article article)
    {
        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteArticleAsync(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article != null)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Article>> GetAllArticlesWithUsersAsync()
    {
        return await _context.Articles
            .Include(a => a.User) 
            .ToListAsync();
    }

    public async Task<Article> GetArticleWithUserByIdAsync(int id)
    {
        return await _context.Articles
            .Include(a => a.User) 
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    public async Task<Article?> GetArticleWithUserAndCommentsByIdAsync(int id)
    {
        return await _context.Articles
            .Include(a => a.User) 
            .Include(a => a.Comments)
                .ThenInclude(c => c.User) 
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
