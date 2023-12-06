using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IUnitOfWork UnitOfWork { get; }

        Task<TEntity> GetAsync(object[] keyValues);
        IQueryable<TEntity> GetAll(bool readOnly = true);

        void Add(TEntity item);
        void Delete(TEntity item);
        void Update(TEntity item);
        void Update(TEntity item, string[] includedProperties);
    }
}
