using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Domain.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository user { get; }
        ICityRepository city { get; }

        IAttendanceRepository attendance { get; }

        IRoomRepository room { get; }

        Task Save();
    }
}
