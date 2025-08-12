using Graduation.Infrustructure.Abstract;
using Graduation.Infrustructure.Context;
using Graduation.Infrustructure.Specifications;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Infrustructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _appDbContext;
        public GenericRepository(ApplicationDbContext context)
        {
            _appDbContext = context;

        }
        public async Task AddAsync(T entity)
        {
            await _appDbContext.Set<T>().AddAsync(entity);

        }

   
        public async Task<int> CountSpecAsync(Expression<Func<T, bool>>? Criteria)
        {
            if (Criteria == null)
                return await _appDbContext .Set<T>().CountAsync();
            return await _appDbContext.Set<T>().Where(Criteria).CountAsync();
        }

        public void Delete(T entity)
        {
            _appDbContext.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            return await _appDbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllSpecAsync(Specification<T> spec)
        {
            return await BaseQuary(spec).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _appDbContext.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetByIdSpecAsync(Specification<T> spec)
        {
            return await BaseQuary(spec).FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            _appDbContext.Update(entity);
        }
        private IQueryable<T> BaseQuary(Specification<T> spec)
        {
            return SpecificaionQueryBuilder.GetQuery<T> (_appDbContext.Set<T>(), spec);
        }
    }
}
