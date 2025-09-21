using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ICityService
    {
        Task<List<city>> GetAll();
        Task<city> GetById(int id);
        Task Create(city model);
        Task Update(city model);
        Task Delete(int id);
    }
}
