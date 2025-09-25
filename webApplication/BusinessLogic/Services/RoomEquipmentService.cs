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

        public async Task<room_equipment> GetById(int roomId)
        {
            if (roomId <= 0) 
                throw new ArgumentNullException(nameof(roomId));

            var roomEquipment = await _repositoryWrapper.roomEquipment.
                FindByCondition(x => x.roomid == roomId);

            if (roomEquipment.Count == 0)
                throw new KeyNotFoundException($"Did not found room with roomId: {roomId}");


            return roomEquipment.First();
        }

        public async Task Create(room_equipment model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.roomEquipment.Create(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id, string equipment)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));
            if (equipment == null)
                throw new ArgumentNullException(nameof(equipment));

            var rooms = await _repositoryWrapper.roomEquipment
                .FindByCondition(x => x.roomid == id && x.equipment == equipment);

            if (rooms.Count == 0)
                throw new KeyNotFoundException($"Room equipment '{equipment}' not found for room ID {id}");
            if (rooms.Count > 1)
                throw new InvalidOperationException($"Found more then one objects with roomid: {id} and equipment: {equipment}");

            await _repositoryWrapper.roomEquipment.Delete(rooms.Single());
            await _repositoryWrapper.Save();
        }
    }
}