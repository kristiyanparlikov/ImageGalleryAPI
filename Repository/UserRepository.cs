using Microsoft.EntityFrameworkCore;
using photo_gallery_api.Entities;

namespace photo_gallery_api.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int userId);
        Task<User> InsertUser(User user);
        Task<bool> DeleteUser(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(u => u.Email == email).SingleOrDefaultAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> InsertUser(User user)
        {
            User result = _context.Users.Add(user).Entity;
            await _context.SaveChangesAsync();
            return result;
            
        }

        public async Task<bool> DeleteUser(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
