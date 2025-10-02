using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAcces.Repositories
{
    public class AttendanceRepository : RepositoryBase<attendance>, IAttendanceRepository
    {

        public AttendanceRepository(LDBContext repostoryContext) : base(repostoryContext)
        {
        }
        public async Task<List<attendance>> GetByUserIdWithDetails(int userId)
        {
            return await RepositoryContext.attendances
                .Include(a => a.user)
                .Include(a => a.lecture)
                .Where(a => a.userid == userId) 
                .AsNoTracking()
                .ToListAsync();
        }
    }
}