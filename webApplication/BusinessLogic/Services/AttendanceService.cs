using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace BusinessLogic.Services
    {
    public class AttendanceService : IAttendanceService
        {
        private IRepositoryWrapper _repositoryWrapper;

        public AttendanceService(IRepositoryWrapper repositoryWrapper)
            {
            _repositoryWrapper = repositoryWrapper;
            }

        public async Task<List<attendance>> GetAll()
            {
            return await _repositoryWrapper.attendance.FindAll();
            }

        public async Task<attendance> GetById(int id)
            {
            var attendance = await _repositoryWrapper.attendance.
                FindByCondition(x => x.attendanceid == id);
            return attendance.First();
            }

        public async Task Create(attendance model)
            {
            await _repositoryWrapper.attendance.Create(model);
            _repositoryWrapper.Save();
            }
        public async Task Update(attendance model)
            {
            await _repositoryWrapper.attendance.Update(model);
            _repositoryWrapper.Save();
            }

        public async Task Delete(int id)
            {
            var attendance = await _repositoryWrapper.attendance
                .FindByCondition(x => x.attendanceid == id);
            _repositoryWrapper.attendance.Delete(attendance.First());
            }
        }
    }