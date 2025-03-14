using FishingForum.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FishingForum.Services.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleByIdAsync(int id);
        Task CreateArticleAsync(Article article);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(int id);
        Task<IEnumerable<Article>> GetAllArticlesWithUsersAsync();
        Task<Article> GetArticleWithUserByIdAsync(int id);
        Task<Article?> GetArticleWithUserAndCommentsByIdAsync(int id);
    }
}
