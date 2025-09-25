using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace BusinessLogic.Services
{
    public class PortfolioService : IportfolioService
    {

        private IRepositoryWrapper _repositoryWrapper;
        public PortfolioService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<portfolio>> GetAll()
        {
            return await _repositoryWrapper.portfolio.FindAll();
        }

        public async Task<portfolio> GetById(int portfolioid)
        {
            if (portfolioid <= 0) 
                throw new ArgumentNullException(nameof(portfolioid));

            var portfolio = await _repositoryWrapper.portfolio.
                FindByCondition(x => x.userid == portfolioid);

            if (portfolio.Count == 0)
                throw new KeyNotFoundException($"Did not found portfolios with portfolioId: {portfolioid}");
            if (portfolio.Count > 1)
                throw new InvalidOperationException($"Found more then one portfolios with portfolioId: {portfolioid}");

            return portfolio.Single();
        }

        public async Task Create(portfolio model)
        {
            if (model == null)
                throw new KeyNotFoundException(nameof(model));

            await _repositoryWrapper.portfolio.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id, string achievement)
        {
            if(id == 0)
                throw new ArgumentNullException(nameof(id));
            if (achievement == null)
                throw new ArgumentNullException(nameof(achievement));

            var portfolio = await _repositoryWrapper.portfolio.FindByCondition(x => x.userid == id && x.achievement == achievement);
            if (portfolio.Count == 0)
                throw new KeyNotFoundException($"Room userId:{id} and achievement: {achievement}");
            if (portfolio.Count > 1)
                throw new InvalidOperationException($"Founde more then one objects");

            await _repositoryWrapper.portfolio.Delete(portfolio.Single());
            await _repositoryWrapper.Save();
        }
    }
}