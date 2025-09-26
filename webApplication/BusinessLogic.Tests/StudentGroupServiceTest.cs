using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation.Results;
using Moq;
using Validators.Interefaces;

namespace BusinessLogic.Tests
{
    public class StudentsGroupServiceTest
    {
        private readonly StudentsGroupService service;
        private readonly Mock<IStudentsGroupRepository> studentsGroupRepositoryMoq;
        private readonly Mock<IStudentGroupValidator> studentGroupValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;

        public StudentsGroupServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            studentsGroupRepositoryMoq = new Mock<IStudentsGroupRepository>();
            studentGroupValidatorMoq = new Mock<IStudentGroupValidator>();

            repositoryWrapperMoq.Setup(x => x.studentsGroup)
                .Returns(studentsGroupRepositoryMoq.Object);

            service = new StudentsGroupService(repositoryWrapperMoq.Object, studentGroupValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            studentsGroupRepositoryMoq.Verify(x => x.Create(It.IsAny<students_group>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectStudentGroups()
        {
            return new List<object []>
            {
                new object [] { new students_group { userid = 0, groupid = 1 } },
                new object [] { new students_group { userid = 1, groupid = 0 } },
                new object [] { new students_group { userid = 0, groupid = 0 } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectStudentGroups))]
        public async Task CreateAsyncStudentGroupShouldNotCreate(students_group model)
        {
            var validationResult = new ValidationResult();

            if (model.userid <= 0)
                validationResult.Errors.Add(new ValidationFailure("userid", "UserId is required"));
            if (model.groupid <= 0)
                validationResult.Errors.Add(new ValidationFailure("groupid", "GroupId is required"));

            studentGroupValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(model));

            studentsGroupRepositoryMoq.Verify(x => x.Create(It.IsAny<students_group>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewStudentGroupShouldCreate()
        {
            var newStudentGroup = new students_group
            {
                userid = 123,
                groupid = 456,
                enrolledat = DateOnly.FromDateTime(DateTime.Now)
            };

            studentGroupValidatorMoq.Setup(x => x.ValidateAsync(newStudentGroup))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newStudentGroup);

            studentsGroupRepositoryMoq.Verify(x => x.Create(It.IsAny<students_group>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }







        [Fact]
        public async Task GetById_WhenGroupIdOrUserIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0, 1));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(1, 0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1, 1));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(1, -1));
        }

        [Fact]
        public async Task GetById_WhenStudentGroupNotFound_ThrowsKeyNotFoundException()
        {
            studentsGroupRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<students_group, bool>>>()))
                .ReturnsAsync(new List<students_group>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999, 888));
            Assert.Contains("Objects with userId: 888 and groupId: 999 not found", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenStudentGroupFound_ReturnsStudentGroup()
        {
            var expected = new students_group
            {
                userid = 100,
                groupid = 200,
                enrolledat = DateOnly.FromDateTime(DateTime.Now)
            };

            studentsGroupRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<students_group, bool>>>()))
                .ReturnsAsync(new List<students_group> { expected });

            var result = await service.GetById(200, 100);

            Assert.Equal(100, result.userid);
            Assert.Equal(200, result.groupid);
            studentsGroupRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<students_group, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenMultipleStudentGroupsFound_ThrowsInvalidOperationException()
        {
            var students = new List<students_group>
            {
                new students_group { userid = 1, groupid = 2 },
                new students_group { userid = 1, groupid = 2 }
            };

            studentsGroupRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<students_group, bool>>>()))
                .ReturnsAsync(students);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.GetById(2, 1));
            Assert.Contains("Found more then one object with userId: 1 and groupId: 2", ex.Message);
        }

        [Fact]
        public async Task GetAll_ReturnsAllStudentGroups()
        {
            var mockGroups = new List<students_group>
            {
                new students_group { userid = 1, groupid = 10 },
                new students_group { userid = 2, groupid = 20 }
            };

            studentsGroupRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockGroups);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result [0].userid);
            Assert.Equal(10, result [0].groupid);
            Assert.Equal(2, result [1].userid);
            Assert.Equal(20, result [1].groupid);
            studentsGroupRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenGroupIdOrUserIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0, 1));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(1, 0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(-1, 1));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(1, -1));
        }

        [Fact]
        public async Task Delete_WhenStudentGroupNotFound_ThrowsKeyNotFoundException()
        {
            studentsGroupRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<students_group, bool>>>()))
                .ReturnsAsync(new List<students_group>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999, 888));
            Assert.Contains("Objects with userId: 888 and groupId: 999 not found", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenStudentGroupFound_DeletesAndSaves()
        {
            var studentGroup = new students_group
            {
                userid = 555,
                groupid = 666
            };

            studentsGroupRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<students_group, bool>>>()))
                .ReturnsAsync(new List<students_group> { studentGroup });

            await service.Delete(666, 555);

            studentsGroupRepositoryMoq.Verify(x => x.Delete(It.IsAny<students_group>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}