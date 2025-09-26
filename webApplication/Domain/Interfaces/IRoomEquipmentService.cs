using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
    {
    public interface IRoomEquipmentService
        {
        Task<List<room_equipment>> GetAll();

        Task<List<room_equipment>> GetById(int id);
        Task Create(room_equipment model);

        Task Delete(int id , string equipment);

        }
    }