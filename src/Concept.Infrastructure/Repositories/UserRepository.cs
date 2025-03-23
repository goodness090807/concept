using Concept.Core.Entities.User;
using Concept.Core.Interfaces.Repositories;
using Concept.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Concept.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddUserAsync(string username, string email, string hashedPassword)
        {
            var user = new UserEntity
            {
                UserName = username,
                Email = email,
                HashedPassword = hashedPassword
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
