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
    public class LectureServiceTest
    {
        private readonly LectureService service;
        private readonly Mock<ILectureRepository> lectureRepositoryMoq;
        private readonly Mock<ILectureValidator> lectureValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly Mock<IUserRepository> userRepositoryMoq;
        private readonly Mock<IRoomRepository> roomRepositoryMoq;

        public LectureServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            lectureRepositoryMoq = new Mock<ILectureRepository>();
            lectureValidatorMoq = new Mock<ILectureValidator>();
            userRepositoryMoq = new Mock<IUserRepository>();
            roomRepositoryMoq = new Mock<IRoomRepository>();

            repositoryWrapperMoq.Setup(x => x.lecture).Returns(lectureRepositoryMoq.Object);
            repositoryWrapperMoq.Setup(x => x.user).Returns(userRepositoryMoq.Object);
            repositoryWrapperMoq.Setup(x => x.room).Returns(roomRepositoryMoq.Object);

            service = new LectureService(repositoryWrapperMoq.Object, lectureValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            lectureRepositoryMoq.Verify(x => x.Create(It.IsAny<lecture>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectLectures()
        {
            return new List<object []>
            {
                new object [] { new lecture { lecturename = "", teacherid = 1 } },
                new object [] { new lecture { lecturename = "Lecture", teacherid = 0 } },
                new object [] { new lecture { lecturename = "", teacherid = 0 } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectLectures))]
        public async Task CreateAsyncLectureShouldNotCreate(lecture model)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(model.lecturename))
                validationResult.Errors.Add(new ValidationFailure("lecturename", "Lecture name is required"));
            if (model.teacherid <= 0)
                validationResult.Errors.Add(new ValidationFailure("teacherid", "Teacher ID is required and must be greater than 0"));

            lectureValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(model));

            lectureRepositoryMoq.Verify(x => x.Create(It.IsAny<lecture>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewLectureShouldCreate()
        {
            var newLecture = new lecture
            {
                lecturename = "Math 101",
                teacherid = 5,
                isactive = true,
                createdat = DateTime.Now
            };

            lectureValidatorMoq.Setup(x => x.ValidateAsync(newLecture))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newLecture);

            lectureRepositoryMoq.Verify(x => x.Create(It.IsAny<lecture>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Update(null));
            Assert.IsType<ArgumentNullException>(ex);
            lectureRepositoryMoq.Verify(x => x.Update(It.IsAny<lecture>()), Times.Never);
        }

        [Fact]
        public async Task GetLecturesByTeacherId_WhenTeacherIdIsZeroOrNegative_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetLecturesByTeacherId(0));
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetLecturesByTeacherId(-3));
        }

        [Fact]
        public async Task GetLecturesByTeacherId_CallsRepositoryWithIsActiveTrueAndReturnsResult()
        {
            var wrapper = repositoryWrapperMoq.Object;
            var repoFromWrapper = wrapper.lecture; 

            Assert.NotNull(repoFromWrapper); 

            var mockLectures = new List<lecture>
    {
        new lecture { lectureid = 10, lecturename = "Algebra", teacherid = 7, isactive = true },
        new lecture { lectureid = 11, lecturename = "Geometry", teacherid = 7, isactive = true }
    };

            lectureRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(mockLectures);

            var result = await service.GetLecturesByTeacherId(7);

            Assert.NotNull(result); 
            Assert.Equal(2, result.Count);
            lectureRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectLectures))]
        public async Task UpdateAsyncLectureShouldNotUpdate(lecture model)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(model.lecturename))
                validationResult.Errors.Add(new ValidationFailure("lecturename", "Lecture name is required"));
            if (model.teacherid <= 0)
                validationResult.Errors.Add(new ValidationFailure("teacherid", "Teacher ID is required and must be greater than 0"));

            lectureValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Update(model));

            lectureRepositoryMoq.Verify(x => x.Update(It.IsAny<lecture>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task UpdateAsyncNewLectureShouldUpdate()
        {
            var updateLecture = new lecture
            {
                lectureid = 10,
                lecturename = "Updated Math",
                teacherid = 5,
                roomid = 3,
                starttime = new TimeOnly(9, 0),
                endtime = new TimeOnly(10, 30)
            };

            var existingLecture = new lecture
            {
                lectureid = 10,
                lecturename = "Old Math",
                teacherid = 2,
                roomid = 1,
                starttime = new TimeOnly(8, 0),
                endtime = new TimeOnly(9, 30)
            };

            lectureRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture> { existingLecture });

            lectureValidatorMoq.Setup(x => x.ValidateAsync(updateLecture))
                .ReturnsAsync(new ValidationResult());

            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user> { new user { userid = 5 } });

            roomRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room, bool>>>()))
                .ReturnsAsync(new List<room> { new room { roomid = 3 } });

            await service.Update(updateLecture);

            lectureRepositoryMoq.Verify(x => x.FindByConditionTraking(It.IsAny<Expression<Func<lecture, bool>>>()), Times.Once);
            userRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()), Times.Once);
            roomRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<room, bool>>>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
            lectureValidatorMoq.Verify(x => x.ValidateAsync(updateLecture), Times.Once);
        }

        [Fact]
        public async Task UpdateAsyncLecture_WhenTeacherNotFound_ThrowsKeyNotFoundException()
        {
            var updateLecture = new lecture
            {
                lectureid = 10,
                lecturename = "Updated Math",
                teacherid = 999,
                roomid = 3
            };

            var existingLecture = new lecture
            {
                lectureid = 10,
                lecturename = "Old Math",
                teacherid = 2
            };

            lectureRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture> { existingLecture });

            lectureValidatorMoq.Setup(x => x.ValidateAsync(updateLecture))
                .ReturnsAsync(new ValidationResult());

            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Update(updateLecture));
            Assert.Contains("Did not found users with userId: 999", ex.Message);
        }

        [Fact]
        public async Task UpdateAsyncLecture_WhenRoomNotFound_ThrowsKeyNotFoundException()
        {
            var updateLecture = new lecture
            {
                lectureid = 10,
                lecturename = "Updated Math",
                teacherid = 5,
                roomid = 999
            };

            var existingLecture = new lecture
            {
                lectureid = 10,
                lecturename = "Old Math",
                teacherid = 2
            };

            lectureRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture> { existingLecture });

            lectureValidatorMoq.Setup(x => x.ValidateAsync(updateLecture))
                .ReturnsAsync(new ValidationResult());

            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user> { new user { userid = 5 } });

            roomRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room, bool>>>()))
                .ReturnsAsync(new List<room>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Update(updateLecture));
            Assert.Contains("Did not found rooms with roomId: 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenLectureIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1));
        }

        [Fact]
        public async Task GetById_WhenLectureNotFound_ThrowsKeyNotFoundException()
        {
            lectureRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999));
            Assert.Contains("Did not found lectures with lectureId: 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenLectureFound_ReturnsLecture()
        {
            var expected = new lecture
            {
                lectureid = 42,
                lecturename = "Physics",
                teacherid = 5,
                isactive = true,
                createdat = DateTime.Now
            };

            lectureRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture> { expected });

            var result = await service.GetById(42);

            Assert.Equal(42, result.lectureid);
            Assert.Equal("Physics", result.lecturename);
            lectureRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsAllLectures()
        {
            var mockLectures = new List<lecture>
            {
                new lecture { lectureid = 1, lecturename = "Math", teacherid = 1 },
                new lecture { lectureid = 2, lecturename = "Chemistry", teacherid = 2 }
            };

            lectureRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockLectures);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Math", result [0].lecturename);
            Assert.Equal("Chemistry", result [1].lecturename);
            lectureRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenLectureIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0));
        }

        [Fact]
        public async Task Delete_WhenLectureNotFound_ThrowsKeyNotFoundException()
        {
            lectureRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999));
            Assert.Contains("Did not found lectures with lectureId: 999", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenMultipleLecturesFound_ThrowsInvalidOperationException()
        {
            var lectures = new List<lecture>
            {
                new lecture { lectureid = 5, lecturename = "Test" },
                new lecture { lectureid = 5, lecturename = "Test2" }
            };

            lectureRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(lectures);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.Delete(5));
            Assert.Contains("found more then one lectures", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenLectureFound_DeletesAndSaves()
        {
            var lectureToDelete = new lecture
            {
                lectureid = 777,
                lecturename = "ToDelete",
                teacherid = 888
            };

            lectureRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<lecture, bool>>>()))
                .ReturnsAsync(new List<lecture> { lectureToDelete });

            await service.Delete(777);

            lectureRepositoryMoq.Verify(x => x.Delete(It.IsAny<lecture>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}