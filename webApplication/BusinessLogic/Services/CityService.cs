using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace BusinessLogic.Services
{
    public class CityService : ICityService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public CityService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<city>> GetAll() 
        {
            return await _repositoryWrapper.city.FindAll();
        }

        public async Task<city> GetById(int id) 
        {
            var city = await _repositoryWrapper.city
                .FindByCondition(x => x.cityid == id);
            return city.First();
        }

        public async Task Create(city model) 
        {
            await _repositoryWrapper.city.Create(model);
            _repositoryWrapper.Save();
        }

        public async Task Update(city model) 
        {
            await _repositoryWrapper.city.Update(model);
            _repositoryWrapper.Save();
        }

        public async Task Delete(int id) 
        {
            var city = await _repositoryWrapper.city.FindByCondition(x => x.cityid == id);
            _repositoryWrapper.city.Delete(city.First());
        }
    }
}
