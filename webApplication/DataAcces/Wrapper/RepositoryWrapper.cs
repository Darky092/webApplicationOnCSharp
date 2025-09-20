using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using DataAcces.Repositories;


namespace DataAcces.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private LDBContext _repoContext;



        private IUserRepository _user;
        public IUserRepository user 
        {
            get 
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }

        public RepositoryWrapper(LDBContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public async Task Save() 
        {
            _repoContext.SaveChanges();
        }
    }
}
