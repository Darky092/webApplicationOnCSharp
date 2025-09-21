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
            var room = await _repositoryWrapper.lecture.
                FindByCondition(x => x.lectureid == id);
            return room.First();
        }

        public async Task Create(lecture model)
        {
            await _repositoryWrapper.lecture.Create(model);
            _repositoryWrapper.Save();
        }
        public async Task Update(lecture model)
        {
            await _repositoryWrapper.lecture.Update(model);
            _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var room = await _repositoryWrapper.lecture
                .FindByCondition(x => x.lectureid == id);
            _repositoryWrapper.lecture.Delete(room.First());
        }
    }
}
