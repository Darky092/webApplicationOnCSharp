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
            await _repositoryWrapper.user.Create(model);
            _repositoryWrapper.Save();
        }

        public async Task Update(user model)
        {
            _repositoryWrapper.user.Update(model);
            _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var user = await _repositoryWrapper.user
                .FindByCondition(x => x.userid == id);

            _repositoryWrapper.user.Delete(user.First());
            _repositoryWrapper.Save();
        }
    }
}