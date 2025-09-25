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
            if (id <= 0)
                throw new ArgumentNullException("Require roomId",nameof(id));
                
            var room = await _repositoryWrapper.room.
                FindByCondition(x => x.roomid == id);

            if (room.Count == 0)
                throw new KeyNotFoundException($"Did not found rooms with roomId: {id}");
            if (room.Count > 1)
                throw new InvalidOperationException($"Found more then one rooms with roomId: {id}");

            return room.First();
        }

        public async Task Create(room model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.room.Create(model);
            await _repositoryWrapper.Save();

        }
        public async Task Update(room model)
        {
            var rooms = await _repositoryWrapper.room.FindByConditionTraking(x => x.roomid == model.roomid);

            if (rooms.Count == 0)
                throw new KeyNotFoundException($"not found {model.roomid}id");
            if (rooms.Count > 1)
                throw new InvalidOperationException($"To many rooms was founded: {rooms.Count}");

            var expected = rooms.Single();
            
            if (model.institutionid != 0)
                expected.institutionid = model.institutionid;
            if (model.roomnumber != null)
                expected.roomnumber = model.roomnumber;

            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("Require roomId",nameof(id));

            var room = await _repositoryWrapper.room
                .FindByCondition(x => x.roomid == id);

            if (room.Count == 0)
                throw new KeyNotFoundException($"Did not found rooms with roomId: {id}");

            if (room.Count > 1)
                throw new InvalidOperationException($"found more then one room with roomId: {id}");

            await _repositoryWrapper.room.Delete(room.First());            
            await _repositoryWrapper.Save();
        }
    }
}