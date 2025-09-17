using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Interfaces;

namespace DataAcces.Models.Repositories
{
    public class UserRepository : RepositoryBase<user>, IUserRepository
    {
        public UserRepository(LDBContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
