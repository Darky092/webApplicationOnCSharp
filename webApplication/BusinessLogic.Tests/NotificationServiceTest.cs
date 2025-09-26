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
    public class NotificationServiceTest
    {
        private readonly NotificationService service;
        private readonly Mock<INotificationRepository> notificationRepositoryMoq;
        private readonly Mock<INotificationValidator> notificationValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;

        public NotificationServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            notificationRepositoryMoq = new Mock<INotificationRepository>();
            notificationValidatorMoq = new Mock<INotificationValidator>();

            repositoryWrapperMoq.Setup(x => x.notification)
                .Returns(notificationRepositoryMoq.Object);

            service = new NotificationService(repositoryWrapperMoq.Object, notificationValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            notificationRepositoryMoq.Verify(x => x.Create(It.IsAny<notification>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectNotifications()
        {
            return new List<object []>
            {
                new object [] { new notification { userid = 0, note = "Test" } },
                new object [] { new notification { userid = 1, note = "" } },
                new object [] { new notification { userid = 0, note = "" } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectNotifications))]
        public async Task CreateAsyncNotificationShouldNotCreate(notification model)
        {
            var validationResult = new ValidationResult();

            if (model.userid <= 0)
                validationResult.Errors.Add(new ValidationFailure("userid", "User ID is required"));
            if (string.IsNullOrEmpty(model.note))
                validationResult.Errors.Add(new ValidationFailure("note", "Note is required"));

            notificationValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(model));

            notificationRepositoryMoq.Verify(x => x.Create(It.IsAny<notification>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewNotificationShouldCreate()
        {
            var newNotification = new notification
            {
                userid = 5,
                note = "You have a new message",
                createdat = DateTime.Now,
                isread = false
            };

            notificationValidatorMoq.Setup(x => x.ValidateAsync(newNotification))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newNotification);

            notificationRepositoryMoq.Verify(x => x.Create(It.IsAny<notification>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Update(null));
            Assert.IsType<ArgumentNullException>(ex);
            notificationRepositoryMoq.Verify(x => x.Update(It.IsAny<notification>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectNotifications))]
        public async Task UpdateAsyncNotificationShouldNotUpdate(notification model)
        {
            var validationResult = new ValidationResult();

            if (model.userid <= 0)
                validationResult.Errors.Add(new ValidationFailure("userid", "User ID is required"));
            if (string.IsNullOrEmpty(model.note))
                validationResult.Errors.Add(new ValidationFailure("note", "Note is required"));

            // Для Update — добавляем проверку isread
            if (!model.isread.HasValue)
                validationResult.Errors.Add(new ValidationFailure("isread", "IsRead is required"));

            notificationValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Update(model));

            notificationRepositoryMoq.Verify(x => x.Update(It.IsAny<notification>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task UpdateAsyncNewNotificationShouldUpdate()
        {
            var updateNotification = new notification
            {
                notificationid = 10,
                userid = 5,
                note = "Updated note",
                isread = true,
                createdat = DateTime.Now
            };

            var existingNotification = new notification
            {
                notificationid = 10,
                userid = 3,
                note = "Old note",
                isread = false,
                createdat = DateTime.Now
            };

            notificationRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<notification, bool>>>()))
                .ReturnsAsync(new List<notification> { existingNotification });

            notificationValidatorMoq.Setup(x => x.ValidateAsync(updateNotification))
                .ReturnsAsync(new ValidationResult());

            await service.Update(updateNotification);

            notificationRepositoryMoq.Verify(x => x.FindByConditionTraking(It.IsAny<Expression<Func<notification, bool>>>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
            notificationValidatorMoq.Verify(x => x.ValidateAsync(updateNotification), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenNotificationIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1));
        }

        [Fact]
        public async Task GetById_WhenNotificationNotFound_ThrowsKeyNotFoundException()
        {
            notificationRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<notification, bool>>>()))
                .ReturnsAsync(new List<notification>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999));
            Assert.Contains("Did not found notifications with id 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenNotificationFound_ReturnsNotification()
        {
            var expected = new notification
            {
                notificationid = 42,
                userid = 5,
                note = "Test note",
                isread = true,
                createdat = DateTime.Now
            };

            notificationRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<notification, bool>>>()))
                .ReturnsAsync(new List<notification> { expected });

            var result = await service.GetById(42);

            Assert.Equal(42, result.notificationid);
            Assert.Equal("Test note", result.note);
            Assert.True(result.isread.Value);
            notificationRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<notification, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsAllNotifications()
        {
            var mockNotifications = new List<notification>
            {
                new notification { notificationid = 1, userid = 1, note = "Note1", isread = false },
                new notification { notificationid = 2, userid = 2, note = "Note2", isread = true }
            };

            notificationRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockNotifications);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Note1", result [0].note);
            Assert.Equal("Note2", result [1].note);
            notificationRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenNotificationIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0));
        }

        [Fact]
        public async Task Delete_WhenNotificationNotFound_ThrowsKeyNotFoundException()
        {
            notificationRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<notification, bool>>>()))
                .ReturnsAsync(new List<notification>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999));
            Assert.Contains("Did not found notification with notificationId 999", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenMultipleNotificationsFound_ThrowsInvalidOperationException()
        {
            var notifications = new List<notification>
            {
                new notification { notificationid = 5, userid = 1, note = "Test" },
                new notification { notificationid = 5, userid = 2, note = "Test" }
            };

            notificationRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<notification, bool>>>()))
                .ReturnsAsync(notifications);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.Delete(5));
            Assert.Contains("Found more then one notiifications", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenNotificationFound_DeletesAndSaves()
        {
            var notificationToDelete = new notification
            {
                notificationid = 777,
                userid = 888,
                note = "To delete",
                isread = false
            };

            notificationRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<notification, bool>>>()))
                .ReturnsAsync(new List<notification> { notificationToDelete });

            await service.Delete(777);

            notificationRepositoryMoq.Verify(x => x.Delete(It.IsAny<notification>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}