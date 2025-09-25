using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace BusinessLogic.Services
{
    public class LectureService : ILectureService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public LectureService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<lecture>> GetAll()
        {
            return await _repositoryWrapper.lecture.FindAll();
        }

        public async Task<lecture> GetById(int id)
        {
            if (id == 0) 
                throw new ArgumentNullException(nameof(id));
            var lecture = await _repositoryWrapper.lecture.
                FindByCondition(x => x.lectureid == id);
            if (lecture == null)
                throw new KeyNotFoundException(nameof(lecture));
            return lecture.Single();
        }

        public async Task Create(lecture model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.lecture.Create(model);
            await _repositoryWrapper.Save();
        }
        public async Task Update(lecture model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            var lectures = await _repositoryWrapper.lecture.FindByConditionTraking(x=> x.lectureid == model.lectureid);
            if (lectures.Count == 0)
                throw new KeyNotFoundException(nameof(lectures));
            if (lectures.Count > 1)
                throw new KeyNotFoundException(nameof(lectures));
            var lecture = lectures.Single();

            if (!string.IsNullOrEmpty(model.lecturename)) lecture.lecturename = model.lecturename;
            if (!string.IsNullOrEmpty(model.description)) lecture.description = model.description;
            if (model.starttime.HasValue) lecture.starttime = model.starttime.Value;
            if (model.endtime.HasValue) lecture.endtime = model.endtime.Value;
            if (model.teacherid != 0) lecture.teacherid = model.teacherid;
            var tryKeepTeacherId = await _repositoryWrapper.user.FindByCondition(x => x.userid == model.teacherid);
            if (tryKeepTeacherId.Count == 0)
                throw new KeyNotFoundException(nameof(tryKeepTeacherId));
            if (model.roomid != 0) lecture.lectureid = model.lectureid;
            var tryKeepRoomIId = await _repositoryWrapper.room.FindByCondition(x => x.roomid == model.roomid); 
            if (tryKeepRoomIId.Count == 0)
                throw new KeyNotFoundException(nameof(tryKeepRoomIId));


            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if(id == 0)
                throw new ArgumentNullException(nameof(id));
            var room = await _repositoryWrapper.lecture
                .FindByCondition(x => x.lectureid == id);
            if (room.Count == 0)
                throw new KeyNotFoundException(nameof(room));
            await _repositoryWrapper.lecture.Delete(room.First());
            await _repositoryWrapper.Save();
        }
    }
}