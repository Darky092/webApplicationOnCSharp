using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Validators.Interefaces;


namespace BusinessLogic.Services
{
    public class StudentsGroupService : IStudentsGroupService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IStudentGroupValidator _studentGroupValidator;

        public StudentsGroupService(IRepositoryWrapper repositoryWrapper, IStudentGroupValidator validator)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _studentGroupValidator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<List<students_group>> GetAll()
        {
            return await _repositoryWrapper.studentsGroup.FindAll();
        }

        public async Task<List<students_group>>GetById(int userId)
        {
            if (userId <= 0)
                throw new ArgumentNullException("Required userId", nameof(userId));

            var studentsGroup = await _repositoryWrapper.studentsGroup
                .FindByCondition(x => x.userid == userId);

            if (studentsGroup.Count == 0)
                throw new KeyNotFoundException($"Objects with userId: {userId}  not found");


            return studentsGroup;
        }

        public async Task Create(students_group model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var valResult = await _studentGroupValidator.ValidateAsync(model);
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

            await _repositoryWrapper.studentsGroup.Create(model);
            try
            {
                await _repositoryWrapper.Save();
            }
            catch
            {
                throw new ArgumentException("Объект с таким id уже существ");
            }

        }


        public async Task<List<lecture>> GetLecturesByUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            return await _repositoryWrapper.studentsGroup.GetLecturesByUserId(userId);
        }


        public async Task Delete(int groupId, int userId)
        {
            if (groupId <= 0)
                throw new ArgumentNullException("Require groupid", nameof(groupId));
            if (userId <= 0)
                throw new ArgumentNullException("Require userid", nameof(userId));

            var studentsGroup = await _repositoryWrapper.studentsGroup
            .FindByCondition(x => x.userid == userId && x.groupid == groupId);

            if (studentsGroup.Count == 0)
                throw new KeyNotFoundException($"Objects with userId: {userId} and groupId: {groupId} not found");

            if (studentsGroup.Count > 1)
                throw new InvalidOperationException($"Found more then one object with userId: {userId} and groupId: {groupId}");
         
            await _repositoryWrapper.studentsGroup.Delete(studentsGroup.Single());
            await _repositoryWrapper.Save();
        }
        public async Task<List<user>> GetStudentsByLectureId(int lectureId)
        {
            if (lectureId <= 0)
                throw new ArgumentException("Invalid lecture ID", nameof(lectureId));

            return await _repositoryWrapper.studentsGroup.GetStudentsByLectureId(lectureId);
        }
    }
}