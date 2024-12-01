using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEvents.Domain.Identity;
using ProEvents.Persistence.Context;
using ProEvents.Persistence.Contratos;

namespace ProEvents.Persistence
{
    public class UserPersist : GeralPersist, IUserPersist
    {
        private readonly ProEventsContext _context;

        public UserPersist(ProEventsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync(); //Users é um item de IdentityDbContext
        }

        public async Task<User> GetUsersByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUsersByUserNameAsync(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserName == userName.ToLower());
        }
    }
}