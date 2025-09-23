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

        public async Task<portfolio> GetById(int id)
            {
            var portfolio = await _repositoryWrapper.portfolio.
                FindByCondition(x => x.userid == id);
            return portfolio.First();
            }

        public async Task Create(portfolio model)
            {
            await _repositoryWrapper.portfolio.Create(model);
            }
        public async Task Update(portfolio model)
            {
            await _repositoryWrapper.portfolio.Update(model);
            }
        public async Task Delete(int id)
            {
            var institution = await _repositoryWrapper.portfolio.FindByCondition(x => x.userid == id);
            _repositoryWrapper.portfolio.Delete(institution.First());
            }
        }
    }