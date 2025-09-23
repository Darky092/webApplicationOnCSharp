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
            await _repositoryWrapper.notification.Create(model);
            _repositoryWrapper.Save();
            }
        public async Task Update(notification model)
            {
            await _repositoryWrapper.notification.Update(model);
            _repositoryWrapper.Save();
            }

        public async Task Delete(int id)
            {
            var room = await _repositoryWrapper.notification
                .FindByCondition(x => x.notificationid == id);
            _repositoryWrapper.notification.Delete(room.First());
            }
        }
    }