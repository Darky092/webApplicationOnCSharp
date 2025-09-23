using Domain.Interfaces;
using Domain.Models;


namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public UserService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<user>> GetAll()
        {
            return await _repositoryWrapper.user.FindAll();
        }

        public async Task<user> GetById(int id)
        {
            var user = await _repositoryWrapper.user
                .FindByCondition(x => x.userid == id);
            return user.First();
        }

        public async Task Create(user model)
        {
            if (model == null) 
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (string.IsNullOrEmpty(model.name)) 
            {
                throw new ArgumentException(nameof(model.name));
            }
            await _repositoryWrapper.user.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(user model)
        {
            await _repositoryWrapper.user.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var user = await _repositoryWrapper.user
                .FindByCondition(x => x.userid == id);

            await _repositoryWrapper.user.Delete(user.First());
            await _repositoryWrapper.Save();
        }
    }
}