using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ILectureService
    {
        Task<List<lecture>> GetAll();
        Task<lecture> GetById(int id);

        Task Create(lecture model);
        Task Update(lecture model);
        Task Delete(int id);
    }
}
