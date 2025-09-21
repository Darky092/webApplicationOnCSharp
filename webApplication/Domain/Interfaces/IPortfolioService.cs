using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IportfolioService
    {
        Task<List<portfolio>> GetAll();
        Task<portfolio> GetById(int id);

        Task Create(portfolio model);
        Task Update(portfolio model);
        Task Delete(int id);
    }
}
