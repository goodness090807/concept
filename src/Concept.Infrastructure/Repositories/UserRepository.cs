using Concept.Core.Entities.User;
using Concept.Core.Entities.User.Enums;
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

        public async Task<int> AddUserAsync(string username, string email, string hashedPassword, UserStatus userStatus)
        {
            var user = new UserEntity
            {
                Email = email,
                UserName = username,
                HashedPassword = hashedPassword,
                UserStatus = userStatus,
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
