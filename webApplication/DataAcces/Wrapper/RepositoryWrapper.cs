using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using DataAcces.Repositories;


namespace DataAcces.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private LDBContext _repoContext;

        private ICityRepository _city;

        private IUserRepository _user;

        private IAttendanceRepository _attendance;

        private IRoomRepository _room;

        private IGroupReposiitory _group;

        private IInstitutionRepository _institution;

        private ILectureRepository _lecture;

        private INotificationRepository _notification;

        private IPortfolioRepository _portfolio;



        public IPortfolioRepository portfolio
        {
            get
            {
                if (_portfolio == null)
                {
                    _portfolio = new PortfolioRepository(_repoContext);
                }
                return _portfolio;
            }
        }

        public INotificationRepository notification 
        {
            get 
            {
                if (_notification == null) 
                {
                    _notification = new NotificationRepository(_repoContext);
                }
                return _notification;
            }
        }

        public ILectureRepository lecture
        {
            get
            {
                if (_lecture == null)
                {
                    _lecture = new LectureRepository(_repoContext);
                }
                return _lecture;
            }
        }

        public IInstitutionRepository institution
        {
            get
            {
                if (_institution == null)
                {
                    _institution = new InstitutionRepository(_repoContext);
                }
                return _institution;
            }
        }


        public IGroupReposiitory group
        {
            get
            {
                if (_group == null)
                {
                    _group = new GroupRepository(_repoContext);
                }
                return _group;
            }
        }

        public IRoomRepository room
        {
            get
            {
                if (_room == null)
                {
                    _room = new RoomRepository(_repoContext);
                }
                return _room;
            }
        }


        public IAttendanceRepository attendance 
        {
            get 
            {
                if (_attendance == null) 
                {
                    _attendance = new AttendanceRepository(_repoContext);
                }
                return _attendance;
            }
        }
        public IUserRepository user
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }

        public ICityRepository city 
        {
            get 
            {
                if (_city == null) 
                {
                _city = new CityRepository(_repoContext);
                }
                return _city;
            }
        }

        public RepositoryWrapper(LDBContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public async Task Save() 
        {
            _repoContext.SaveChanges();
        }
    }
}
