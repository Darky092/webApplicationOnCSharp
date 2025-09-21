using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Interfaces;

namespace DataAcces.Repositories
{
    public class RoomEquipmentRepository : RepositoryBase<room_equipment> , IRoomEquipmentRepository
    {
        public RoomEquipmentRepository(LDBContext repositoryContext) : base(repositoryContext) { }
    }
}
