using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAcces.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected LDBContext RepositoryContext { get; set; }

        public RepositoryBase(LDBContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        public async Task<List<T>> FindAll() => await RepositoryContext.Set<T>().AsNoTracking().ToListAsync();
        public async Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression) => await RepositoryContext.Set<T>().Where(expression).AsNoTracking().ToListAsync();
        //Use This func for Update by id
        public async Task<List<T>> FindByConditionTraking(Expression<Func<T, bool>> expression) => await RepositoryContext.Set<T>().Where(expression).ToListAsync();
        public async Task Create(T entity) => await RepositoryContext.Set<T>().AddAsync(entity);
        //Update the most rotten func in this frame work do not use this. 
        public async Task Update(T entity) => RepositoryContext.Set<T>().Update(entity);
        public async Task Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);



    }
}