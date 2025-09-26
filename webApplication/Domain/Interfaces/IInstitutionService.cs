using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IInstitutionService
    {
        Task<List<institution>> GetAll();
        Task<institution> GetById(int id);

        Task Create(institution model);
        Task Update(institution model);
        Task Delete(int id);

    }
}