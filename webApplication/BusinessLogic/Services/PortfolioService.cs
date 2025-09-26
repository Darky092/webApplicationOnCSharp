using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Validators.Interefaces;

namespace BusinessLogic.Services
{
    public class PortfolioService : IportfolioService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IPortfolioValidator _portfolioValidator;

        public PortfolioService(IRepositoryWrapper repositoryWrapper, IPortfolioValidator validator)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _portfolioValidator = validator ?? throw new ArgumentNullException(nameof(validator));
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
                throw new ArgumentNullException(nameof(model));

            var valResult = await _portfolioValidator.ValidateAsync(model);
            if (!valResult.IsValid)
            {
                string errors = string.Join("; ", valResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"{errors}");
            }

            await _repositoryWrapper.portfolio.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id, string achievement)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));
            if (achievement == null)
                throw new ArgumentNullException(nameof(achievement));

            var portfolio = await _repositoryWrapper.portfolio.FindByCondition(x => x.userid == id && x.achievement == achievement);
            if (portfolio.Count == 0)
                throw new KeyNotFoundException($"Portfolio for userId: {id} and achievement: {achievement} not found");
            if (portfolio.Count > 1)
                throw new InvalidOperationException($"Found more then one objects with userId: {id} and achievement: {achievement}");

            await _repositoryWrapper.portfolio.Delete(portfolio.Single());
            await _repositoryWrapper.Save();
        }
    }
}