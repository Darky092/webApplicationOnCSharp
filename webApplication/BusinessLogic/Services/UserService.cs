using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;
using Domain.Models;
using Validators.Interefaces;


namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IUserValidator _userValidator;

        public UserService(IRepositoryWrapper repositoryWrapper, IUserValidator validator)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _userValidator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<List<user>> GetAll()
        {
            return await _repositoryWrapper.user.FindAll();
        }

        public async Task<user> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            var user = await _repositoryWrapper.user
                .FindByCondition(x => x.userid == id);

            if(user.Count == 0)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            return user.Single();
        }

        public async Task Create(user model)
        {
            if (model == null) 
                throw new ArgumentNullException(nameof(model));

            var valResult = await _userValidator.ValidateAsync(model);
            if (!valResult.IsValid) 
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }
            await _repositoryWrapper.user.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(user model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            
            var valResult = await _userValidator.ValidateAsync(model);
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

            
            var users = await _repositoryWrapper.user.FindByConditionTraking(x => x.userid == model.userid);
            
            if (users.Count == 0)
                throw new KeyNotFoundException($"User with ID {model.userid} not found.");
            if (users.Count > 1)
                throw new InvalidOperationException($"Multiple users found with ID {model.userid}. This should not happen.");

            var existing = users.Single();

            if (model.name != null) existing.name = model.name;
            if (model.surname != null) existing.surname = model.surname;
            if (model.patronymic != null) existing.patronymic = model.patronymic;
            if (model.email != null) existing.email = model.email;
            if (model.passwordhash != null) existing.passwordhash = model.passwordhash;
            if (model.role != null) existing.role = model.role;
            if (model.avatar != null) existing.avatar = model.avatar;
            if (model.telephonnumber != null) existing.telephonnumber = model.telephonnumber;


            if (model.isactive.HasValue) existing.isactive = model.isactive.Value;




            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id), "User ID is required.");

            var user = await _repositoryWrapper.user
                .FindByCondition(x => x.userid == id);

            if (user.Count == 0)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            if (user.Count > 1)
                throw new InvalidOperationException($"Finded more then one user with ID: {id}");

            await _repositoryWrapper.user.Delete(user.Single());
            await _repositoryWrapper.Save();
        }
    }
}