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

        public async Task<lectures_group> GetById(int lectureid)
        {
            if (lectureid <= 0)
                throw new ArgumentNullException(nameof(lectureid));

            var lectures_group = await _repositoryWrapper.lecturesGroups.
                FindByCondition(x => x.lectureid == lectureid);

            if (lectures_group.Count == 0)
                throw new KeyNotFoundException($"Did not found objects with lectured: {lectureid}");
            if (lectures_group.Count > 1)
                throw new InvalidOperationException("Found more then one objects");

            return lectures_group.Single();
        }

        public async Task Create(lectures_group model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));
            await _repositoryWrapper.lecturesGroups.Create(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int lectureid, int groupid)
        {
            if(lectureid <= 0)
                throw new ArgumentNullException(nameof(lectureid));
            if (groupid <= 0)
                throw new ArgumentNullException(nameof(groupid));

            var lectures_group = await _repositoryWrapper.lecturesGroups
                .FindByCondition(x => x.lectureid == lectureid && x.groupid == groupid);

            if (lectures_group.Count == 0)
                throw new KeyNotFoundException($"Did not found objects with lectured: {lectureid} and groupid: {groupid}");
            if (lectures_group.Count > 1)
                throw new InvalidOperationException("Found more then one objects"); ;


            await _repositoryWrapper.lecturesGroups.Delete(lectures_group.Single());
            await _repositoryWrapper.Save();
        }
    }
}