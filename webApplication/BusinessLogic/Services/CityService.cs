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
            if (id == 0)
                throw new ArgumentNullException(nameof(id));
            var city = await _repositoryWrapper.city
                .FindByCondition(x => x.cityid == id);
            if(city.Count == 0)
                throw new  KeyNotFoundException(nameof(city));
            return city.First();
        }

        public async Task Create(city model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));
            await _repositoryWrapper.city.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(city model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var cities = await _repositoryWrapper.city.FindByConditionTraking(x =>x.cityid ==model.cityid);

            if (cities.Count == 0)
                throw new KeyNotFoundException(nameof(cities));
            if (cities.Count > 1)
                throw new InvalidOperationException(nameof(cities));
            var city = cities.Single();

            if (model.cityname != null)  city.cityname = model.cityname;
            if (model.postalcode != null) city.postalcode = model.postalcode;
            if (model.country != null) city.country = model.country;

            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var city = await _repositoryWrapper.city.FindByCondition(x => x.cityid == id);
            if (city.Count == 0)
                throw new KeyNotFoundException(nameof(city));

            await _repositoryWrapper.city.Delete(city.Single());
            await _repositoryWrapper.Save();
        }
    }
}