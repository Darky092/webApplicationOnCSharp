using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Interfaces;
using DataAcces.Models;
using DataAcces.Models.Repositories;

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

        public void Save() 
        {
            _repoContext.SaveChanges();
        }
    }
}
