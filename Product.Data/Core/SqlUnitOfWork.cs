using Product.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Data.Core
{
    public class SqlUnitOfWork : IQueryableUnitOfWork
    {
        #region Members

        private readonly ProductContext _context;
      

        public string ConnectionString => _context?.Database.GetDbConnection().ConnectionString;

        #endregion

        #region Constructor

        public SqlUnitOfWork(ProductContext context)
        {
            _context = context;;
        }

        #endregion

        #region IQueryableUnitOfWork members        

        public DbSet<TEntity> CreateSet<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            //attach and set as unchanged
            //attach automatically set to uncahnged ??
            _context.Entry(item).State = EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : class
        {
            //_context.Entry(item).State = EntityState.Detached;
            //_context.Entry(item).State = EntityState.Unchanged;
            _context.Entry(item).State = EntityState.Modified;
            //_context.Entry(item).CurrentValues.Properties
            //    .ToList()
            //    .ForEach(p => _context.Entry(item).Property(p.Name).IsModified = true);
        }

        public void SetModified<TEntity>(TEntity item, string[] includedProperties) where TEntity : class
        {
            _context.Entry(item).State = EntityState.Detached;
            _context.Entry(item).State = EntityState.Unchanged;
            _context.Entry(item).CurrentValues.Properties
                .Where(p => includedProperties.Contains(p.Name))
                .ToList()
                .ForEach(p => _context.Entry(item).Property(p.Name).IsModified = true);
        }

        public int Commit()
        {
            var result = _context.SaveChanges();
            return result;
        }

        public async Task<int> CommitAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .ToList();
            foreach (var entity in changedEntriesCopy)
            {
                _context.Entry(entity.Entity).State = EntityState.Detached;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        #endregion

    }
}
