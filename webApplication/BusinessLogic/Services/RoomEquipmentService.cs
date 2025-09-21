using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace BusinessLogic.Services
{
    public class RoomEquipmentService : IRoomEquipmentService
    {

        private IRepositoryWrapper _repositoryWrapper;

        public RoomEquipmentService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<room_equipment>> GetAll()
        {
            return await _repositoryWrapper.roomEquipment.FindAll();
        }

        public async Task<room_equipment> GetById(int id)
        {
            var roomEquipment = await _repositoryWrapper.roomEquipment.
                FindByCondition(x => x.roomid == id);
            return roomEquipment.First();
        }

        public async Task Create(room_equipment model)
        {
            await _repositoryWrapper.roomEquipment.Create(model);
            _repositoryWrapper.Save();
        }
        public async Task Update(room_equipment model)
        {
            await _repositoryWrapper.roomEquipment.Update(model);
            _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var room = await _repositoryWrapper.roomEquipment
                .FindByCondition(x => x.roomid == id);
            _repositoryWrapper.roomEquipment.Delete(room.First());
        }
    }
}
