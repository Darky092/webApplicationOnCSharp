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

        public async Task<group> GetById(int groupid)
        {
            if (groupid <= 0)
                throw new ArgumentNullException(nameof(groupid));

            var group = await _repositoryWrapper.group.
                FindByCondition(x => x.groupid == groupid);
            return group.Single();
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
                throw new KeyNotFoundException($"Did not found groups with groupId: {model.groupid}");

            if (groups.Count > 1)
                throw new InvalidOperationException("found more then one group");

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
        public async Task Delete(int groupid)
        {
            if (groupid <= 0)
                throw new ArgumentNullException(nameof(groupid));

            var group = await _repositoryWrapper.group.FindByCondition(x => x.groupid == groupid);

            if(group.Count == 0)
                throw new KeyNotFoundException($"Did not found groups with groupId: {groupid}");
            if (group.Count > 1)
                throw new InvalidOperationException("found more then one group");

            await _repositoryWrapper.group.Delete(group.Single());
            await _repositoryWrapper.Save();
        }
    }
}