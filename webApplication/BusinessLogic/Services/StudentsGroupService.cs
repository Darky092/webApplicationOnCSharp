using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace BusinessLogic.Services
{
    public class StudentsGroupService : IStudentsGroupService
    {

        private IRepositoryWrapper _repositoryWrapper;

        public StudentsGroupService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<students_group>> GetAll()
        {
            return await _repositoryWrapper.studentsGroup.FindAll();
        }

        public async Task<students_group> GetById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));
            var studentsGroup = await _repositoryWrapper.studentsGroup.
                FindByCondition(x => x.userid == id);
            return studentsGroup.First() ?? throw new KeyNotFoundException(nameof(id));
        }

        public async Task Create(students_group model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            await _repositoryWrapper.studentsGroup.Create(model);
            await _repositoryWrapper.Save();
        }
        public async Task Update(students_group model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            await _repositoryWrapper.studentsGroup.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));
            var studentsGroup = await _repositoryWrapper.studentsGroup
                .FindByCondition(x => x.userid == id);
            if(studentsGroup.Count == 0)
                throw new KeyNotFoundException(nameof(id));
            await _repositoryWrapper.studentsGroup.Delete(studentsGroup.First());
            await _repositoryWrapper.Save();
        }
    }
}