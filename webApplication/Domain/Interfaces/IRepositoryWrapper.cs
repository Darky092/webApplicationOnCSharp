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

        IGroupReposiitory group { get; }

        IInstitutionRepository institution { get; }

        ILectureRepository lecture { get; }

        INotificationRepository notification { get; }

        IPortfolioRepository portfolio { get; }

        IRoomEquipmentRepository roomEquipment { get; }

        IStudentsGroupRepository studentsGroup { get; }

        ILecturesGroupsRepository lecturesGroups { get; }

        Task Save();
    }
}
