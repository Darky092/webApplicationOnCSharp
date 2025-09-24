using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

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

        public async Task<notification> GetById(int id)
        {
            var room = await _repositoryWrapper.notification.
                FindByCondition(x => x.notificationid == id);
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
                throw new ArgumentException(nameof(notifications));


            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var room = await _repositoryWrapper.notification
                .FindByCondition(x => x.notificationid == id);
            
            await _repositoryWrapper.notification.Delete(room.First());
            await _repositoryWrapper.Save();
        }
    }
}