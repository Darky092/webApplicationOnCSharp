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

        Task<List<students_group>> GetById(int userId);
        Task Create(students_group model);
        Task Delete(int groupId, int userId);

        Task<List<lecture>> GetLecturesByUserId(int userId);

        Task<List<user>> GetStudentsByLectureId(int lectureId);
    }
}