using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace DataAcces.Repositories
{
    public class PortfolioRepository : RepositoryBase<portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(LDBContext repositoryContext) : base(repositoryContext) { }
    }
}
