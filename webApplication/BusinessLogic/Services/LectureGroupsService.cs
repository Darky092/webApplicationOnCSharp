using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace BusinessLogic.Services
{
    public class LectureGroupsService : ILecturesGroupsService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public LectureGroupsService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<lectures_group>> GetAll()
        {
            return await _repositoryWrapper.lecturesGroups.FindAll();
        }

        public async Task<lectures_group> GetById(int id)
        {
            var lectures_group = await _repositoryWrapper.lecturesGroups.
                FindByCondition(x => x.lectureid == id);
            return lectures_group.First();
        }

        public async Task Create(lectures_group model)
        {
            await _repositoryWrapper.lecturesGroups.Create(model);
            await _repositoryWrapper.Save();
        }
        public async Task Update(lectures_group model)
        {
            await _repositoryWrapper.lecturesGroups.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var lectures_group = await _repositoryWrapper.lecturesGroups
                .FindByCondition(x => x.lectureid == id);
            await _repositoryWrapper.lecturesGroups.Delete(lectures_group.First());
            await _repositoryWrapper.Save();
        }
    }
}