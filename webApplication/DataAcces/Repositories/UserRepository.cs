using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAcces.Repositories
{
    public class UserRepository : RepositoryBase<user>, IUserRepository
    {
        public UserRepository(LDBContext repositoryContext) : base(repositoryContext)
        {

        }
        public async Task<user> GetByIdWithToken(int userId) =>
            await RepositoryContext.Set<user>().Include(x => x.RefreshTokens).AsNoTracking().FirstOrDefaultAsync(x => x.userid == userId);
        public async Task<user> GetByEmailWithToken(string email) =>
            await RepositoryContext.Set<user>().Include(x => x.RefreshTokens).AsNoTracking().FirstOrDefaultAsync(x => x.email == email);
    }
}