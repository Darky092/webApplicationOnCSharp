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
    public class RoomServiceTest
    {
        private readonly RoomService service;
        private readonly Mock<IRoomRepository> roomRepositoryMoq;
        private readonly Mock<IRoomValidator> roomValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;

        public RoomServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            roomRepositoryMoq = new Mock<IRoomRepository>();
            roomValidatorMoq = new Mock<IRoomValidator>();

            repositoryWrapperMoq.Setup(x => x.room)
                .Returns(roomRepositoryMoq.Object);

            service = new RoomService(repositoryWrapperMoq.Object, roomValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            roomRepositoryMoq.Verify(x => x.Create(It.IsAny<room>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectRooms()
        {
            return new List<object []>
            {
                new object [] { new room { roomnumber = "", institutionid = 1 } },
                new object [] { new room { roomnumber = "A101", institutionid = 0 } },
                new object [] { new room { roomnumber = "", institutionid = 0 } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectRooms))]
        public async Task CreateAsyncRoomShouldNotCreate(room model)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(model.roomnumber))
                validationResult.Errors.Add(new ValidationFailure("roomnumber", "Room number is required"));
            if (model.institutionid <= 0)
                validationResult.Errors.Add(new ValidationFailure("institutionid", "Institution ID is required"));

            roomValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(model));

            roomRepositoryMoq.Verify(x => x.Create(It.IsAny<room>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewRoomShouldCreate()
        {
            var newRoom = new room
            {
                roomnumber = "A101",
                institutionid = 5,
            };

            roomValidatorMoq.Setup(x => x.ValidateAsync(newRoom))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newRoom);

            roomRepositoryMoq.Verify(x => x.Create(It.IsAny<room>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Update(null));
            Assert.IsType<ArgumentNullException>(ex);
            roomRepositoryMoq.Verify(x => x.Update(It.IsAny<room>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectRooms))]
        public async Task UpdateAsyncRoomShouldNotUpdate(room model)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(model.roomnumber))
                validationResult.Errors.Add(new ValidationFailure("roomnumber", "Room number is required"));
            if (model.institutionid <= 0)
                validationResult.Errors.Add(new ValidationFailure("institutionid", "Institution ID is required"));

            roomValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Update(model));

            roomRepositoryMoq.Verify(x => x.Update(It.IsAny<room>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task UpdateAsyncNewRoomShouldUpdate()
        {
            var updateRoom = new room
            {
                roomid = 10,
                roomnumber = "B202",
                institutionid = 15,
            };

            var existingRoom = new room
            {
                roomid = 10,
                roomnumber = "OldRoom",
                institutionid = 5,
            };

            roomRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<room, bool>>>()))
                .ReturnsAsync(new List<room> { existingRoom });

            roomValidatorMoq.Setup(x => x.ValidateAsync(updateRoom))
                .ReturnsAsync(new ValidationResult());

            await service.Update(updateRoom);

            roomRepositoryMoq.Verify(x => x.FindByConditionTraking(It.IsAny<Expression<Func<room, bool>>>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
            roomValidatorMoq.Verify(x => x.ValidateAsync(updateRoom), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1));
        }

        [Fact]
        public async Task GetById_WhenRoomNotFound_ThrowsKeyNotFoundException()
        {
            roomRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room, bool>>>()))
                .ReturnsAsync(new List<room>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999));
            Assert.Contains("Did not found rooms with roomId: 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenRoomFound_ReturnsRoom()
        {
            var expectedRoom = new room
            {
                roomid = 42,
                roomnumber = "C303",
                institutionid = 100
            };

            roomRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room, bool>>>()))
                .ReturnsAsync(new List<room> { expectedRoom });

            var result = await service.GetById(42);

            Assert.Equal(42, result.roomid);
            Assert.Equal("C303", result.roomnumber);
            Assert.Equal(100, result.institutionid);
            roomRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<room, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenMultipleRoomsFound_ThrowsInvalidOperationException()
        {
            var rooms = new List<room>
            {
                new room { roomid = 5, roomnumber = "A1" },
                new room { roomid = 5, roomnumber = "A2" }
            };

            roomRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room, bool>>>()))
                .ReturnsAsync(rooms);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.GetById(5));
            Assert.Contains("Found more then one rooms with roomId: 5", ex.Message);
        }

        [Fact]
        public async Task GetAll_ReturnsAllRooms()
        {
            var mockRooms = new List<room>
            {
                new room { roomid = 1, roomnumber = "A1", institutionid = 1 },
                new room { roomid = 2, roomnumber = "B2", institutionid = 2 }
            };

            roomRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockRooms);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("A1", result [0].roomnumber);
            Assert.Equal("B2", result [1].roomnumber);
            roomRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0));
        }

        [Fact]
        public async Task Delete_WhenRoomNotFound_ThrowsKeyNotFoundException()
        {
            roomRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room, bool>>>()))
                .ReturnsAsync(new List<room>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999));
            Assert.Contains("Did not found rooms with roomId: 999", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenMultipleRoomsFound_ThrowsInvalidOperationException()
        {
            var rooms = new List<room>
            {
                new room { roomid = 5, roomnumber = "A1" },
                new room { roomid = 5, roomnumber = "A2" }
            };

            roomRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room, bool>>>()))
                .ReturnsAsync(rooms);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.Delete(5));
            Assert.Contains("found more then one room with roomId: 5", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenRoomFound_DeletesAndSaves()
        {
            var roomToDelete = new room { roomid = 777, roomnumber = "D404", institutionid = 888 };

            roomRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room, bool>>>()))
                .ReturnsAsync(new List<room> { roomToDelete });

            await service.Delete(777);

            roomRepositoryMoq.Verify(x => x.Delete(It.IsAny<room>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}