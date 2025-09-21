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
