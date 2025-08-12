using Graduation.Infrustructure.Abstract;
using Graduation.Infrustructure.Context;
using Graduation.Infrustructure.Repositories;
using Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Infrustructure.UnitOfWorks
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork, IAsyncDisposable, IDisposable
    {
        private readonly ApplicationDbContext _context = context;
        private readonly Hashtable _repositories = new Hashtable();

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            // Call synchronous dispose methods if any
            // Otherwise, just suppress finalization
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseModel
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepository<TEntity>)_repositories[type]!;
        }
    }
}
