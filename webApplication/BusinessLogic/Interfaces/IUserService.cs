using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Models;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<List<user>> GetAll();
        Task<user> GetById(int id);
        Task Create(user model);
        Task Update(user model);
        Task Delete(int id);
    }
}
