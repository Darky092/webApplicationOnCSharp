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
            var studentsGroup = await _repositoryWrapper.studentsGroup.
                FindByCondition(x => x.userid == id);
            return studentsGroup.First();
            }

        public async Task Create(students_group model)
            {
            await _repositoryWrapper.studentsGroup.Create(model);
            _repositoryWrapper.Save();
            }
        public async Task Update(students_group model)
            {
            await _repositoryWrapper.studentsGroup.Update(model);
            _repositoryWrapper.Save();
            }

        public async Task Delete(int id)
            {
            var studentsGroup = await _repositoryWrapper.studentsGroup
                .FindByCondition(x => x.userid == id);
            _repositoryWrapper.studentsGroup.Delete(studentsGroup.First());
            }
        }
    }