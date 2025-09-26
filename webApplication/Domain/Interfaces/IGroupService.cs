using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IGroupService
    {
        Task<List<group>> GetAll();

        Task<group> GetById(int id);

        Task Create(group model);
        Task Update(group model);
        Task Delete(int id);

    }
}