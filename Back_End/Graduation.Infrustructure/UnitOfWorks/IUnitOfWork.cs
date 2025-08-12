using Graduation.Infrustructure.Abstract;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Infrustructure.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseModel;
        public Task<int> CompleteAsync();
    }
}
