using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using DataAcces.Models;
using DataAcces.Wrapper;
using DataAcces.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public UserService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<user>> GetAll()
        {
            return _repositoryWrapper.user.FindAll().ToListAsync();
        }

        public Task<user> GetById(int id)
        {
            var user = _repositoryWrapper.user
                .FindByCondition(x => x.userid == id).First();
            return Task.FromResult(user);
        }

        public Task Create(user model)
        {
            _repositoryWrapper.user.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(user model)
        {
            _repositoryWrapper.user.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var user = _repositoryWrapper.user
                .FindByCondition(x => x.userid == id).First();

            _repositoryWrapper.user.Delete(user);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
