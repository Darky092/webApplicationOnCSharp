using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Validators.Interefaces;

namespace BusinessLogic.Services
{
    public class LectureService : ILectureService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private ILectureValidator _lectureValidator;

        public LectureService(IRepositoryWrapper repositoryWrapper, ILectureValidator validator)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _lectureValidator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<List<lecture>> GetAll()
        {
            return await _repositoryWrapper.lecture.FindAll();
        }

        public async Task<lecture> GetById(int lectureId)
        {
            if (lectureId <= 0)
                throw new ArgumentNullException(nameof(lectureId));

            var lecture = await _repositoryWrapper.lecture.
                FindByCondition(x => x.lectureid == lectureId);

            if (lecture.Count == 0)
                throw new KeyNotFoundException($"Did not found lectures with lectureId: {lectureId}");

            if (lecture.Count > 1)
                throw new InvalidOperationException($"Found more then one lectures");

            return lecture.Single();
        }

        public async Task Create(lecture model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var valResult = await _lectureValidator.ValidateAsync(model);
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

            await _repositoryWrapper.lecture.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(lecture model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var valResult = await _lectureValidator.ValidateAsync(model);
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

            var lectures = await _repositoryWrapper.lecture.FindByConditionTraking(x => x.lectureid == model.lectureid);

            if (lectures.Count == 0)
                throw new KeyNotFoundException($"Did not found lectures with lectureId: {model.lectureid}");
            if (lectures.Count > 1)
                throw new InvalidOperationException($"found more then one lectures");

            var lecture = lectures.Single();

            if (!string.IsNullOrEmpty(model.lecturename)) lecture.lecturename = model.lecturename;
            if (!string.IsNullOrEmpty(model.description)) lecture.description = model.description;
            if (model.starttime.HasValue) lecture.starttime = model.starttime.Value;
            if (model.endtime.HasValue) lecture.endtime = model.endtime.Value;

            if (model.teacherid > 0)
            {
                lecture.teacherid = model.teacherid;
                var teacher = await _repositoryWrapper.user.FindByCondition(x => x.userid == model.teacherid);
                if (teacher.Count == 0)
                    throw new KeyNotFoundException($"Did not found users with userId: {model.teacherid}");
            }

            if (model.roomid != 0)
            {
                lecture.roomid = model.roomid;
                var room = await _repositoryWrapper.room.FindByCondition(x => x.roomid == model.roomid);
                if (room.Count == 0)
                    throw new KeyNotFoundException($"Did not found rooms with roomId: {model.roomid}");
            }

            await _repositoryWrapper.Save();
        }

        public async Task Delete(int lectureid)
        {
            if (lectureid <= 0)
                throw new ArgumentNullException(nameof(lectureid));

            var lecture = await _repositoryWrapper.lecture
                .FindByCondition(x => x.lectureid == lectureid);

            if (lecture.Count == 0)
                throw new KeyNotFoundException($"Did not found lectures with lectureId: {lectureid}");

            if (lecture.Count > 1)
                throw new InvalidOperationException($"found more then one lectures");

            await _repositoryWrapper.lecture.Delete(lecture.First());
            await _repositoryWrapper.Save();
        }
        public async Task<List<lecture>> GetLecturesByTeacherId(int teacherId)
        {
            if (teacherId <= 0)
                throw new ArgumentException("Invalid teacher ID", nameof(teacherId));

            return await _repositoryWrapper.lecture
                .FindByCondition(l => l.teacherid == teacherId && l.isactive == true);
        }

    }
}