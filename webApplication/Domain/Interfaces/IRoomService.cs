using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IRoomService
    {
        Task<List<room>> GetAll();

        Task<room> GetById(int id);

        Task Create(room model);

        Task Update(room model);

        Task Delete(int id);
    }
}