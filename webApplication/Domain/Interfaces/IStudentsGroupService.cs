using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
    {
    public interface IStudentsGroupService
        {
        Task<List<students_group>> GetAll();

        Task<students_group> GetById(int id);
        Task Create(students_group model);
        Task Update(students_group model);
        Task Delete(int id);
        }
    }