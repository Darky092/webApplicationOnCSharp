using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;


namespace BusinessLogic.Services
{
    public class InstitutionService : IInstitutionService
    {
        private IRepositoryWrapper _repositoryWrapper;
        public InstitutionService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<institution>> GetAll()
        {
            return await _repositoryWrapper.institution.FindAll();
        }

        public async Task<institution> GetById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var institution = await _repositoryWrapper.institution.
                FindByCondition(x => x.institutionid == id);

            if (institution.Count == 0)
                throw new KeyNotFoundException(nameof(institution));
            return institution.First();
        }

        public async Task Create(institution model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));
            await _repositoryWrapper.institution.Create(model);
            await _repositoryWrapper.Save();
        }
        public async Task Update(institution model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var institutions = await _repositoryWrapper.institution.FindByConditionTraking(x => x.institutionid == model.institutionid);
            var institution = institutions.Single(); 
            if(institutions.Count == 0)
                throw new KeyNotFoundException(nameof(institutions));
            if (institutions.Count > 1)
                throw new KeyNotFoundException(nameof(institutions));
            if(model.institutionname != null) institution.institutionname = model.institutionname;
            if(model.street != null) institution.street = model.street;
            if(model.phone != null) institution.phone = model.phone;
            if(model.website != null) institution.website = model.website;
            if(model.cityid != null) institution.cityid = model.cityid;
            var tryKeepCityId = await _repositoryWrapper.city.FindByCondition(x => x.cityid == model.cityid);
            if (tryKeepCityId.Count == 0)
                throw new KeyNotFoundException(nameof(tryKeepCityId));


            await _repositoryWrapper.Save();
        }
        public async Task Delete(int id)
        {
            if(id == 0)
                throw new ArgumentNullException(nameof(id));
            var institution = await _repositoryWrapper.institution.FindByCondition(x => x.institutionid == id);
            if (institution.Count == 0)
                throw new KeyNotFoundException(nameof(institution));
            await _repositoryWrapper.institution.Delete(institution.First());
            await _repositoryWrapper.Save();
        }
    }
}