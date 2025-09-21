using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ILecturesGroupsService
    {
        Task<List<lectures_group>> GetAll();
        Task<lectures_group> GetById(int id);

        Task Create(lectures_group model);
        Task Update(lectures_group model);
        Task Delete(int id);
    }
}
