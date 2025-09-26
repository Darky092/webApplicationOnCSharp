using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Validators.Interefaces;

namespace BusinessLogic.Services
{
    public class InstitutionService : IInstitutionService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IInstitutionValidator _institutionValidator;

        public InstitutionService(IRepositoryWrapper repositoryWrapper, IInstitutionValidator validator)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _institutionValidator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<List<institution>> GetAll()
        {
            return await _repositoryWrapper.institution.FindAll();
        }

        public async Task<institution> GetById(int institutionId)
        {
            if (institutionId <= 0)
                throw new ArgumentNullException(nameof(institutionId));

            var institution = await _repositoryWrapper.institution.
                FindByCondition(x => x.institutionid == institutionId);

            if (institution.Count == 0)
                throw new KeyNotFoundException($"Did not found institution with institutionId: {institutionId} ");

            if (institution.Count > 1)
                throw new InvalidOperationException("Found more then one institution");

            return institution.First();
        }

        public async Task Create(institution model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var valResult = await _institutionValidator.ValidateAsync(model);
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

            await _repositoryWrapper.institution.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(institution model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var valResult = await _institutionValidator.ValidateAsync(model); 
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

            var institutions = await _repositoryWrapper.institution.FindByConditionTraking(x => x.institutionid == model.institutionid);

            if (institutions.Count == 0)
                throw new KeyNotFoundException($"Did not found institution with institutionId: {model.institutionid}");
            if (institutions.Count > 1)
                throw new InvalidOperationException("Found more then one institution");

            var institution = institutions.Single();

            if (model.institutionname != null) institution.institutionname = model.institutionname;
            if (model.street != null) institution.street = model.street;
            if (model.phone != null) institution.phone = model.phone;
            if (model.website != null) institution.website = model.website;
            if (model.cityid.HasValue) institution.cityid = model.cityid.Value;

            if (model.cityid.HasValue)
            {
                var tryKeepCityId = await _repositoryWrapper.city.FindByCondition(x => x.cityid == model.cityid.Value);
                if (tryKeepCityId.Count == 0)
                    throw new KeyNotFoundException($"Did not found city with cityId: {model.cityid.Value}");
            }

            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var institution = await _repositoryWrapper.institution.FindByCondition(x => x.institutionid == id);

            if (institution.Count == 0)
                throw new KeyNotFoundException($"Did not found institution with institutionId: {id}");

            if (institution.Count > 1)
                throw new InvalidOperationException("Found more then one institution");

            await _repositoryWrapper.institution.Delete(institution.First());
            await _repositoryWrapper.Save();
        }
    }
}