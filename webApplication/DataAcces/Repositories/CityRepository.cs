using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Interfaces;

namespace DataAcces.Repositories
{
    public class CityRepository : RepositoryBase<city> , ICityRepository
    {
        public CityRepository(LDBContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
