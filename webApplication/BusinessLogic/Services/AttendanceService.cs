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

        public async Task<attendance> GetById(int attendanceid)
        {
            if (attendanceid <= 0)
                throw new ArgumentNullException(nameof(attendanceid));

            var attendance = await _repositoryWrapper.attendance.
                FindByCondition(x => x.attendanceid == attendanceid);

            return attendance.Single();
        }

        public async Task Create(attendance model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.attendance.Create(model);
            await _repositoryWrapper.Save();
        }
        public async Task Update(attendance model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var attendances = await _repositoryWrapper.attendance.FindByConditionTraking(x => x.attendanceid == model.attendanceid);
            if (attendances.Count == 0)
                throw new KeyNotFoundException($"Did not found attendance with attendanceid: {model.attendanceid}");

            if (attendances.Count > 1)
                throw new InvalidOperationException("Found more then one attendance");

            var attendance = attendances.Single();

            if (model.lectureid != 0) attendance.lectureid = model.lectureid;
            if (model.userid != 0) attendance.userid = model.userid;
            if (model.ispresent.HasValue) attendance.ispresent = model.ispresent.Value;
            if (model.note != null) attendance.note = model.note;

            var tryGetLectureId = await _repositoryWrapper.lecture.FindByCondition(x => x.lectureid == model.lectureid);
            var tryGetUserId = await _repositoryWrapper.user.FindByCondition(x => x.userid == model.userid);

            if (tryGetLectureId.Count == 0)
                throw new KeyNotFoundException($"Did not found attendance with attendanceid: {model.lectureid}");
            if (tryGetUserId.Count == 0)
                throw new KeyNotFoundException($"Did not found attendance with attendanceid: {model.userid}");


            await _repositoryWrapper.Save();
        }

        public async Task Delete(int attendanceid)
        {
            if(attendanceid <= 0)
                throw new ArgumentNullException(nameof(attendanceid));

            var attendance = await _repositoryWrapper.attendance
                .FindByCondition(x => x.attendanceid == attendanceid);
            if (attendance.Count == 0)
                throw new KeyNotFoundException($"Did not found attendance with attendanceid: {attendanceid}");
            if (attendance.Count > 1)
                throw new InvalidOperationException("Found more then one attendance");

            await _repositoryWrapper.attendance.Delete(attendance.First());
            await _repositoryWrapper.Save();
        }
    }
}