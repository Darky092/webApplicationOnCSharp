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


        public async Task<int> DeleteByCondition(Expression<Func<students_group, bool>> predicate)
        {
            int deleted = await RepositoryContext.students_groups
                .Where(predicate)
                .ExecuteDeleteAsync(); 
            return deleted;
        }
    }
}