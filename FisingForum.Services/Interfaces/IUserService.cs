using FishingForum.Domain.Entities;
using System.Threading.Tasks;

namespace FishingForum.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
    }
}
