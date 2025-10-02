using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAcces.Repositories
{
    public class StudentGroupRepository : RepositoryBase<students_group>, IStudentsGroupRepository
    {
        public StudentGroupRepository(LDBContext repositoryContext) : base(repositoryContext)
        {

        }
        public async Task<List<lecture>> GetLecturesByUserId(int userId)
        {
            return await (from sg in RepositoryContext.students_groups
                          join lg in RepositoryContext.lectures_groups on sg.groupid equals lg.groupid
                          join l in RepositoryContext.lectures on lg.lectureid equals l.lectureid
                          where sg.userid == userId
                          select l)
                         .ToListAsync();
        }


        public async Task<int> DeleteByCondition(Expression<Func<students_group, bool>> predicate)
        {
            int deleted = await RepositoryContext.students_groups
                .Where(predicate)
                .ExecuteDeleteAsync();
            return deleted;
        }

        public async Task<List<user>> GetStudentsByLectureId(int lectureId)
        {
            return await (from lg in RepositoryContext.lectures_groups
                          join sg in RepositoryContext.students_groups on lg.groupid equals sg.groupid
                          join u in RepositoryContext.users on sg.userid equals u.userid
                          where lg.lectureid == lectureId && u.role == "Student"
                          select u)
                         .ToListAsync();
        }
    }
}