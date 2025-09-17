using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Interfaces;

namespace DataAcces.Wrapper
{
    public interface IRepositoryWrapper
    {
        IUserRepository user { get; }
        void Save();
    }
}
