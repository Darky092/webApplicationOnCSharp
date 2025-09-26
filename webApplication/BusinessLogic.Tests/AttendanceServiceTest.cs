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
    public class AttendanceServiceTest
    {
        private readonly AttendanceService service;
        private readonly Mock<IAttendanceRepository> attendanceRepositoryMoq;
        private readonly Mock<IAttendanceValidator> attendanceValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly Mock<ILectureRepository> lectureRepositoryMoq;
        private readonly Mock<IUserRepository> userRepositoryMoq;

        public AttendanceServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            attendanceRepositoryMoq = new Mock<IAttendanceRepository>();
            attendanceValidatorMoq = new Mock<IAttendanceValidator>();
            lectureRepositoryMoq = new Mock<ILectureRepository>();
            userRepositoryMoq = new Mock<IUserRepository>();


            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user>());

            lectureRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture>());


            repositoryWrapperMoq.Setup(x => x.attendance).Returns(attendanceRepositoryMoq.Object);
            repositoryWrapperMoq.Setup(x => x.lecture).Returns(lectureRepositoryMoq.Object);
            repositoryWrapperMoq.Setup(x => x.user).Returns(userRepositoryMoq.Object);

            service = new AttendanceService(repositoryWrapperMoq.Object, attendanceValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            attendanceRepositoryMoq.Verify(x => x.Create(It.IsAny<attendance>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectAttendances()
        {
            return new List<object []>
            {
                new object [] { new attendance { lectureid = 0, userid = 5 } },
                new object [] { new attendance { lectureid = 5, userid = 0 } },
                new object [] { new attendance { lectureid = 0, userid = 0 } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectAttendances))]
        public async Task CreateAsyncAttendanceShouldNotCreate(attendance model)
        {
            var validationResult = new ValidationResult();

            if (model.lectureid <= 0)
                validationResult.Errors.Add(new ValidationFailure("lectureid", "Lecture ID is required and must be greater than 0"));
            if (model.userid <= 0)
                validationResult.Errors.Add(new ValidationFailure("userid", "User ID is required and must be greater than 0"));

            attendanceValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(model));

            attendanceRepositoryMoq.Verify(x => x.Create(It.IsAny<attendance>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewAttendanceShouldCreate()
        {
            var newAttendance = new attendance
            {
                lectureid = 10,
                userid = 5,
                ispresent = true,
                note = "Present",
                recordedat = DateTime.Now
            };

            attendanceValidatorMoq.Setup(x => x.ValidateAsync(newAttendance))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newAttendance);

            attendanceRepositoryMoq.Verify(x => x.Create(It.IsAny<attendance>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Update(null));
            Assert.IsType<ArgumentNullException>(ex);
            attendanceRepositoryMoq.Verify(x => x.Update(It.IsAny<attendance>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectAttendances))]
        public async Task UpdateAsyncAttendanceShouldNotUpdate(attendance model)
        {
            var validationResult = new ValidationResult();

            if (model.lectureid <= 0)
                validationResult.Errors.Add(new ValidationFailure("lectureid", "Lecture ID is required and must be greater than 0"));
            if (model.userid <= 0)
                validationResult.Errors.Add(new ValidationFailure("userid", "User ID is required and must be greater than 0"));

            attendanceValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Update(model));

            attendanceRepositoryMoq.Verify(x => x.Update(It.IsAny<attendance>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task UpdateAsyncNewAttendanceShouldUpdate()
        {
            var updateAttendance = new attendance
            {
                attendanceid = 10,
                lectureid = 15,
                userid = 8,
                ispresent = false,
                note = "Late"
            };

            var existingAttendance = new attendance
            {
                attendanceid = 10,
                lectureid = 10,
                userid = 5,
                ispresent = true,
                note = "Present"
            };

            attendanceRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<attendance, bool>>>()))
                .ReturnsAsync(new List<attendance> { existingAttendance });

            attendanceValidatorMoq.Setup(x => x.ValidateAsync(updateAttendance))
                .ReturnsAsync(new ValidationResult());

            lectureRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture> { new lecture { lectureid = 15 } });

            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user> { new user { userid = 8 } });

            await service.Update(updateAttendance);

            attendanceRepositoryMoq.Verify(x => x.FindByConditionTraking(It.IsAny<Expression<Func<attendance, bool>>>()), Times.Once);
            lectureRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()), Times.Once);
            userRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
            attendanceValidatorMoq.Verify(x => x.ValidateAsync(updateAttendance), Times.Once);
        }

        [Fact]
        public async Task UpdateAsyncAttendance_WhenLectureNotFound_ThrowsKeyNotFoundException()
        {
            var updateAttendance = new attendance
            {
                attendanceid = 10,
                lectureid = 999,
                userid = 8
            };

            var existingAttendance = new attendance
            {
                attendanceid = 10,
                lectureid = 10,
                userid = 5
            };

            attendanceRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<attendance, bool>>>()))
                .ReturnsAsync(new List<attendance> { existingAttendance });

            attendanceValidatorMoq.Setup(x => x.ValidateAsync(updateAttendance))
                .ReturnsAsync(new ValidationResult());

            lectureRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Update(updateAttendance));
            Assert.Contains("Did not found lecture with lectureId: 999", ex.Message);
        }

        [Fact]
        public async Task UpdateAsyncAttendance_WhenUserNotFound_ThrowsKeyNotFoundException()
        {
            var updateAttendance = new attendance
            {
                attendanceid = 10,
                lectureid = 11,
                userid = 999
            };

            var existingAttendance = new attendance
            {
                attendanceid = 10,
                lectureid = 10,
                userid = 5
            };
            //test
            attendanceRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<attendance, bool>>>()))
                .ReturnsAsync(new List<attendance> { existingAttendance });

            attendanceValidatorMoq.Setup(x => x.ValidateAsync(updateAttendance))
                .ReturnsAsync(new ValidationResult());


            lectureRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture> { new lecture { lectureid = 15 } });


            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Update(updateAttendance));
            Assert.Contains("Did not found user with userId: 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenAttendanceIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1));
        }

        [Fact]
        public async Task GetById_WhenAttendanceNotFound_ThrowsKeyNotFoundException()
        {
            attendanceRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<attendance, bool>>>()))
                .ReturnsAsync(new List<attendance>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999));
            Assert.Contains("Did not found attendance with attendanceid: 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenAttendanceFound_ReturnsAttendance()
        {
            var expected = new attendance
            {
                attendanceid = 42,
                lectureid = 10,
                userid = 5,
                ispresent = true,
                note = "Present"
            };

            attendanceRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<attendance, bool>>>()))
                .ReturnsAsync(new List<attendance> { expected });

            var result = await service.GetById(42);

            Assert.Equal(42, result.attendanceid);
            Assert.Equal(10, result.lectureid);
            Assert.Equal(5, result.userid);
            attendanceRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<attendance, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsAllAttendances()
        {
            var mockAttendances = new List<attendance>
            {
                new attendance { attendanceid = 1, lectureid = 1, userid = 1, ispresent = true },
                new attendance { attendanceid = 2, lectureid = 2, userid = 2, ispresent = false }
            };

            attendanceRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockAttendances);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result [0].attendanceid);
            Assert.Equal(2, result [1].attendanceid);
            attendanceRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenAttendanceIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0));
        }

        [Fact]
        public async Task Delete_WhenAttendanceNotFound_ThrowsKeyNotFoundException()
        {
            attendanceRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<attendance, bool>>>()))
                .ReturnsAsync(new List<attendance>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999));
            Assert.Contains("Did not found attendance with attendanceid: 999", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenMultipleAttendancesFound_ThrowsInvalidOperationException()
        {
            var attendances = new List<attendance>
            {
                new attendance { attendanceid = 5, lectureid = 1, userid = 1 },
                new attendance { attendanceid = 5, lectureid = 2, userid = 2 }
            };

            attendanceRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<attendance, bool>>>()))
                .ReturnsAsync(attendances);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.Delete(5));
            Assert.Contains("Found more then one attendance", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenAttendanceFound_DeletesAndSaves()
        {
            var attendanceToDelete = new attendance
            {
                attendanceid = 777,
                lectureid = 888,
                userid = 999,
                ispresent = true
            };

            attendanceRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<attendance, bool>>>()))
                .ReturnsAsync(new List<attendance> { attendanceToDelete });

            await service.Delete(777);

            attendanceRepositoryMoq.Verify(x => x.Delete(It.IsAny<attendance>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}