using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using webApplication.Contracts.attendance;


namespace Domain.Interfaces
{
    public interface IAttendanceService
    {
        Task<List<attendance>> GetAll();

        Task<attendance> GetById(int id);

        Task Create(attendance model);
        Task Update(attendance model);
        Task Delete(int id);

        Task<List<AttendanceDetailsDto>> GetAttendanceByUserId(int userId);

        Task UpsertAttendance(CreateAttendanceRequest request);
    }
}