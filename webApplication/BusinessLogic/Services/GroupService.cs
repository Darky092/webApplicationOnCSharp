using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

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
            var group = await _repositoryWrapper.group.
                FindByCondition(x => x.groupid == id);
            return group.First();
            }

        public async Task Create(group model)
            {
            await _repositoryWrapper.group.Create(model);
            await _repositoryWrapper.Save();
        }
        public async Task Update(group model)
            {
            await _repositoryWrapper.group.Update(model);
            await _repositoryWrapper.Save();
        }
        public async Task Delete(int id)
            {
            var group = await _repositoryWrapper.group.FindByCondition(x => x.groupid == id);
            await _repositoryWrapper.group.Delete(group.First());
            await _repositoryWrapper.Save();
        }
        }
    }