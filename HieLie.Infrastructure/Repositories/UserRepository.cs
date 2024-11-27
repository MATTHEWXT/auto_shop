using HieLie.Domain.Entities;
using HieLie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly ShopDbContext _dbContext;

        public UserRepository(ShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByFirstName(string fName) 
        {
            return await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.FirstName == fName) ?? throw new InvalidOperationException("Entity not found");
        }
    }
}
