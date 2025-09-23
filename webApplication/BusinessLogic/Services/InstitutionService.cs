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
            var institution = await _repositoryWrapper.institution.
                FindByCondition(x => x.institutionid == id);
            return institution.First();
            }

        public async Task Create(institution model)
            {
            await _repositoryWrapper.institution.Create(model);
            }
        public async Task Update(institution model)
            {
            await _repositoryWrapper.institution.Update(model);
            }
        public async Task Delete(int id)
            {
            var institution = await _repositoryWrapper.institution.FindByCondition(x => x.institutionid == id);
            _repositoryWrapper.institution.Delete(institution.First());
            }
        }
    }