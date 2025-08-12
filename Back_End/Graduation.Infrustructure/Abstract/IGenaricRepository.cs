using Graduation.Infrustructure.Specifications;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Infrustructure.Abstract
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task<T?> GetByIdSpecAsync(Specification<T> spec);
        Task<IEnumerable<T>> GetAllSpecAsync(Specification<T> spec);
        Task<int> CountSpecAsync(Expression<Func<T, bool>>? Criteria);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
    }

}
