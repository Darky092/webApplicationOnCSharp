using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Localization;

namespace BusinessLogic.Services
{
    public class GroupService : IGroupService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public GroupService(IRepositoryWrapper repositoryWraper)
        {
            _repositoryWrapper = repositoryWraper;
        }

        public async Task<List<group>> GetAll()
        {
            return await _repositoryWrapper.group.FindAll();
        }

        public async Task<group> GetById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var group = await _repositoryWrapper.group.
                FindByCondition(x => x.groupid == id);
            return group.First();
        }

        public async Task Create(group model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.group.Create(model);
            await _repositoryWrapper.Save();
        }
        public async Task Update(group model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var groups = await _repositoryWrapper.group.FindByConditionTraking(x => x.groupid == model.groupid);

            if (groups.Count == 0)
                throw new KeyNotFoundException(nameof(groups));

            if (groups.Count > 1)
                throw new KeyNotFoundException(nameof(groups));

            var group = groups.Single();
            if (model.groupname != null) group.groupname = model.groupname;
            if (model.course != 0) group.course = model.course;
            if (model.curatorid != 0) group.curatorid = model.curatorid;
            if (model.specialty != null) group.specialty = model.specialty;
            if (model.institutionid != 0) group.institutionid = model.institutionid;

            var tryGetInstitutionId = await _repositoryWrapper.institution.FindByCondition(x => x.institutionid == model.institutionid);

            if (tryGetInstitutionId.Count == 0)
                throw new KeyNotFoundException(nameof(tryGetInstitutionId));

            var tryGetCuratorId = await _repositoryWrapper.user.FindByCondition(x => x.userid == model.curatorid);

            if (tryGetCuratorId.Count == 0)
                throw new KeyNotFoundException(nameof(tryGetCuratorId));

            await _repositoryWrapper.Save();
        }
        public async Task Delete(int id)
        {
            if (id == 0)
                throw new KeyNotFoundException(nameof(id));

            var group = await _repositoryWrapper.group.FindByCondition(x => x.groupid == id);

            if(group.Count == 0)
                throw new KeyNotFoundException(nameof(group));

            await _repositoryWrapper.group.Delete(group.Single());
            await _repositoryWrapper.Save();
        }
    }
}