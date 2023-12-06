using Microsoft.EntityFrameworkCore;
using Product.Domain.Interface;

namespace Product.Data.Core
{
    public interface IQueryableUnitOfWork
        : IUnitOfWork
    {
        DbSet<TEntity> CreateSet<TEntity>() where TEntity : class;
        void Attach<TEntity>(TEntity item) where TEntity : class;
        void SetModified<TEntity>(TEntity item) where TEntity : class;
        void SetModified<TEntity>(TEntity item, string[] includedProperties) where TEntity : class;
    }
}
