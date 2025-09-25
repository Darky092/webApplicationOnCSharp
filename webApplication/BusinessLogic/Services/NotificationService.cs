using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace BusinessLogic.Services
{
    public class NotificationService : INotificationService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public NotificationService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<notification>> GetAll()
        {
            return await _repositoryWrapper.notification.FindAll();
        }

        public async Task<notification> GetById(int notficationId)
        {
            if (notficationId <= 0)
                throw new ArgumentNullException(nameof(notficationId));

            var room = await _repositoryWrapper.notification.
                FindByCondition(x => x.notificationid == notficationId);

            if (room.Count == 0)
                throw new KeyNotFoundException($"Did not found notifications with id {notficationId}");
            if (room.Count > 1)
                throw new InvalidOperationException($"Found more then notifications with id {notficationId}");

            return room.First();
        }

        public async Task Create(notification model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.notification.Create(model);
            await _repositoryWrapper.Save();
        }
        public async Task Update(notification model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));

            var notifications = await _repositoryWrapper.notification.FindByConditionTraking(x => x.notificationid == model.notificationid);

            if (notifications.Count == 0)
                throw new KeyNotFoundException($"Did not found notifications with notificationId: {model.notificationid}");
            if (notifications.Count > 1)
                throw new InvalidOperationException($"Found more then one notifications");

            var OneNotification = notifications.Single();

            if(model.userid != 0) OneNotification.userid = model.userid;
            if(model.note != null) OneNotification.note = model.note;
            if(model.isread.HasValue) OneNotification.isread = model.isread.Value;
             
            

            await _repositoryWrapper.Save();
        }

        public async Task Delete(int notificationId)
        {
            if(notificationId == 0)
                throw new ArgumentNullException(nameof(notificationId));

            var room = await _repositoryWrapper.notification
                .FindByCondition(x => x.notificationid == notificationId);

            if(room.Count == 0)
                throw new KeyNotFoundException($"Did not found notification with notificationId {notificationId}");
            if(room.Count > 1)
                throw new InvalidOperationException($"Found more then one notiifications");


            await _repositoryWrapper.notification.Delete(room.First());
            await _repositoryWrapper.Save();
        }
    }
}