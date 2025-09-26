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
    public class NotificationService : INotificationService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private INotificationValidator _notificationValidator;

        public NotificationService(IRepositoryWrapper repositoryWrapper, INotificationValidator validator)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _notificationValidator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<List<notification>> GetAll()
        {
            return await _repositoryWrapper.notification.FindAll();
        }

        public async Task<notification> GetById(int notificationId)
        {
            if (notificationId <= 0)
                throw new ArgumentNullException(nameof(notificationId));

            var notification = await _repositoryWrapper.notification.
                FindByCondition(x => x.notificationid == notificationId);

            if (notification.Count == 0)
                throw new KeyNotFoundException($"Did not found notifications with id {notificationId}");
            if (notification.Count > 1)
                throw new InvalidOperationException($"Found more then notifications with id {notificationId}");

            return notification.First();
        }

        public async Task Create(notification model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var valResult = await _notificationValidator.ValidateAsync(model);
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

            await _repositoryWrapper.notification.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(notification model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var valResult = await _notificationValidator.ValidateAsync(model);
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

            var notifications = await _repositoryWrapper.notification.FindByConditionTraking(x => x.notificationid == model.notificationid);

            if (notifications.Count == 0)
                throw new KeyNotFoundException($"Did not found notifications with notificationId: {model.notificationid}");
            if (notifications.Count > 1)
                throw new InvalidOperationException($"Found more then one notifications");

            var OneNotification = notifications.Single();

            if (model.userid != 0) OneNotification.userid = model.userid;
            if (model.note != null) OneNotification.note = model.note;
            if (model.isread.HasValue) OneNotification.isread = model.isread.Value;

            await _repositoryWrapper.Save();
        }

        public async Task Delete(int notificationId)
        {
            if (notificationId == 0)
                throw new ArgumentNullException(nameof(notificationId));

            var notification = await _repositoryWrapper.notification
                .FindByCondition(x => x.notificationid == notificationId);

            if (notification.Count == 0)
                throw new KeyNotFoundException($"Did not found notification with notificationId {notificationId}");
            if (notification.Count > 1)
                throw new InvalidOperationException($"Found more then one notiifications");

            await _repositoryWrapper.notification.Delete(notification.First());
            await _repositoryWrapper.Save();
        }
    }
}