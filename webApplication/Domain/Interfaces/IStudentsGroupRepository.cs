using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IStudentsGroupRepository : IRepositoryBase<students_group>
    {

        //This func delete object whitout FindByCondition query (delete in one query) // more optimization then simple Delete()
        //DOES NOT USE THIS METHOD
        Task<int> DeleteByCondition(Expression<Func<students_group, bool>> predicate);

        Task<List<lecture>> GetLecturesByUserId(int userId);

        Task<List<user>> GetStudentsByLectureId(int lectureId);
        
    }
}