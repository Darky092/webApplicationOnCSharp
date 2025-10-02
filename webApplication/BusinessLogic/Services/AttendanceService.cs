using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webApplication.Contracts.attendance;
using Domain.Interfaces;
using Domain.Models;
using Validators.Interefaces;

namespace BusinessLogic.Services
{
    public class AttendanceService : IAttendanceService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IAttendanceValidator _attendanceValidator;

        public AttendanceService(IRepositoryWrapper repositoryWrapper, IAttendanceValidator validator)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _attendanceValidator = validator ?? throw new ArgumentNullException(nameof(validator));
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

            if (attendance.Count == 0)
                throw new KeyNotFoundException($"Did not found attendance with attendanceid: {attendanceid}");

            if (attendance.Count > 1)
                throw new InvalidOperationException("Found more then one attendance");

            return attendance.Single();
        }
        public async Task<List<AttendanceDetailsDto>> GetAttendanceByUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than zero.", nameof(userId));

            var attendances = await _repositoryWrapper.attendance.GetByUserIdWithDetails(userId);

            var result = attendances.Select(a => new AttendanceDetailsDto
            {
                AttendanceId = a.attendanceid,
                Username = $"{a.user.name} {(string.IsNullOrEmpty(a.user.surname) ? "" : a.user.surname)}",
                LectureName = a.lecture.lecturename,
                LectureCreatedAt = a.lecture.createdat, 
                IsPresent = a.ispresent,
                Note = a.note,
                RecordedAt = a.recordedat
            }).ToList();

            return result;
        }

        public async Task Create(attendance model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var valResult = await _attendanceValidator.ValidateAsync(model);
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

            await _repositoryWrapper.attendance.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(attendance model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var valResult = await _attendanceValidator.ValidateAsync(model);
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

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

            if (model.lectureid != 0)
            {
                var tryGetLectureId = await _repositoryWrapper.lecture.FindByCondition(x => x.lectureid == model.lectureid);
                if (tryGetLectureId.Count == 0)
                    throw new KeyNotFoundException($"Did not found lecture with lectureId: {model.lectureid}");
            }

            if (model.userid != 0)
            {
                var tryGetUserId = await _repositoryWrapper.user.FindByCondition(x => x.userid == model.userid);
                if (tryGetUserId.Count == 0)
                    throw new KeyNotFoundException($"Did not found user with userId: {model.userid}");
            }

            await _repositoryWrapper.Save();
        }

        public async Task Delete(int attendanceid)
        {
            if (attendanceid <= 0)
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
        public async Task UpsertAttendance(CreateAttendanceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            
            var existing = await _repositoryWrapper.attendance
                .FindByCondition(a => a.lectureid == request.lectureid && a.userid == request.userid);

            if (existing.Any())
            {
               
                var att = existing.First();
                att.ispresent = request.ispresent;
                att.note = request.note;
                await _repositoryWrapper.attendance.Update(att);
            }
            else
            {
                
                var newAtt = new attendance
                {
                    lectureid = request.lectureid,
                    userid = request.userid,
                    ispresent = request.ispresent,
                    note = request.note
                };
                await _repositoryWrapper.attendance.Create(newAtt);
            }

            await _repositoryWrapper.Save();
        }
    }
}