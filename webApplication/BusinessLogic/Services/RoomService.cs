using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace BusinessLogic.Services
    {
    public class RoomService : IRoomService
        {
        private IRepositoryWrapper _repositoryWrapper;

        public RoomService(IRepositoryWrapper repositoryWrapper)
            {
            _repositoryWrapper = repositoryWrapper;
            }

        public async Task<List<room>> GetAll()
            {
            return await _repositoryWrapper.room.FindAll();
            }

        public async Task<room> GetById(int id)
            {
            var room = await _repositoryWrapper.room.
                FindByCondition(x => x.roomid == id);
            return room.First();
            }

        public async Task Create(room model)
            {
            await _repositoryWrapper.room.Create(model);
            _repositoryWrapper.Save();
            _repositoryWrapper.Save();
            }
        public async Task Update(room model)
            {
            await _repositoryWrapper.room.Update(model);
            _repositoryWrapper.Save();
            }

        public async Task Delete(int id)
            {
            var room = await _repositoryWrapper.room
                .FindByCondition(x => x.roomid == id);
            _repositoryWrapper.room.Delete(room.First());
            }
        }
    }