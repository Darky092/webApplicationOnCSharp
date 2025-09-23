using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
    {
    public interface IAttendanceService
        {
        Task<List<attendance>> GetAll();

        Task<attendance> GetById(int id);

        Task Create(attendance model);
        Task Update(attendance model);
        Task Delete(int id);
        }
    }