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
    public class RoomEquipmentServiceTest
    {
        private readonly RoomEquipmentService service;
        private readonly Mock<IRoomEquipmentRepository> roomEquipmentRepositoryMoq;
        private readonly Mock<IRoomEquipmentValidator> roomEquipmentValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;

        public RoomEquipmentServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            roomEquipmentRepositoryMoq = new Mock<IRoomEquipmentRepository>();
            roomEquipmentValidatorMoq = new Mock<IRoomEquipmentValidator>();

            repositoryWrapperMoq.Setup(x => x.roomEquipment)
                .Returns(roomEquipmentRepositoryMoq.Object);

            service = new RoomEquipmentService(repositoryWrapperMoq.Object, roomEquipmentValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            roomEquipmentRepositoryMoq.Verify(x => x.Create(It.IsAny<room_equipment>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectRoomEquipments()
        {
            return new List<object []>
            {
                new object [] { new room_equipment { roomid = 0, equipment = "Chair" } },
                new object [] { new room_equipment { roomid = 1, equipment = "" } },
                new object [] { new room_equipment { roomid = 0, equipment = "" } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectRoomEquipments))]
        public async Task CreateAsyncRoomEquipmentShouldNotCreate(room_equipment model)
        {
            var validationResult = new ValidationResult();

            if (model.roomid <= 0)
                validationResult.Errors.Add(new ValidationFailure("roomid", "Room ID is required"));
            if (string.IsNullOrEmpty(model.equipment))
                validationResult.Errors.Add(new ValidationFailure("equipment", "Equipment name is required"));

            roomEquipmentValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(model));

            roomEquipmentRepositoryMoq.Verify(x => x.Create(It.IsAny<room_equipment>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewRoomEquipmentShouldCreate()
        {
            var newRoomEquipment = new room_equipment
            {
                roomid = 5,
                equipment = "Projector"
            };

            roomEquipmentValidatorMoq.Setup(x => x.ValidateAsync(newRoomEquipment))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newRoomEquipment);

            roomEquipmentRepositoryMoq.Verify(x => x.Create(It.IsAny<room_equipment>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenRoomIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1));
        }

        [Fact]
        public async Task GetById_WhenRoomEquipmentNotFound_ThrowsKeyNotFoundException()
        {
            roomEquipmentRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room_equipment, bool>>>()))
                .ReturnsAsync(new List<room_equipment>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999));
            Assert.Contains("Did not found room with roomId: 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenRoomEquipmentFound_ReturnsRoomEquipment()
        {
            var expected = new room_equipment
            {
                roomid = 42,
                equipment = "Table"
            };

            roomEquipmentRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room_equipment, bool>>>()))
                .ReturnsAsync(new List<room_equipment> { expected });

            var result = await service.GetById(42);

            Assert.Equal(42, result.First().roomid);
            Assert.Equal("Table", result.First().equipment);
            roomEquipmentRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<room_equipment, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsAllRoomEquipments()
        {
            var mockEquipments = new List<room_equipment>
            {
                new room_equipment { roomid = 1, equipment = "Chair" },
                new room_equipment { roomid = 2, equipment = "Desk" }
            };

            roomEquipmentRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockEquipments);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Chair", result [0].equipment);
            Assert.Equal("Desk", result [1].equipment);
            roomEquipmentRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenRoomIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0, "Chair"));
        }

        [Fact]
        public async Task Delete_WhenEquipmentIsNull_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(1, null));
        }

        [Fact]
        public async Task Delete_WhenRoomEquipmentNotFound_ThrowsKeyNotFoundException()
        {
            roomEquipmentRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room_equipment, bool>>>()))
                .ReturnsAsync(new List<room_equipment>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999, "Chair"));
            Assert.Contains("Room equipment 'Chair' not found for room ID 999", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenMultipleRoomEquipmentsFound_ThrowsInvalidOperationException()
        {
            var equipments = new List<room_equipment>
            {
                new room_equipment { roomid = 5, equipment = "Chair" },
                new room_equipment { roomid = 5, equipment = "Chair" }
            };

            roomEquipmentRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room_equipment, bool>>>()))
                .ReturnsAsync(equipments);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.Delete(5, "Chair"));
            Assert.Contains("Found more then one objects with roomid: 5 and equipment: Chair", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenRoomEquipmentFound_DeletesAndSaves()
        {
            var equipmentToDelete = new room_equipment
            {
                roomid = 777,
                equipment = "Whiteboard"
            };

            roomEquipmentRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<room_equipment, bool>>>()))
                .ReturnsAsync(new List<room_equipment> { equipmentToDelete });

            await service.Delete(777, "Whiteboard");

            roomEquipmentRepositoryMoq.Verify(x => x.Delete(It.IsAny<room_equipment>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}