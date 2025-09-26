using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface INotificationService
    {


        Task<List<notification>> GetAll();
        Task<notification> GetById(int id);

        Task Create(notification model);
        Task Update(notification model);
        Task Delete(int id);


    }
}