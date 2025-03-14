using FishingForum.Domain.Entities;
using FishingForum.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FishingForum.DB;

namespace FishingForum.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
