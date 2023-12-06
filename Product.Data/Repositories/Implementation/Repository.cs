using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.Data.Core;
using Product.Domain.Interface;

namespace Product.Data.Repositories.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly IQueryableUnitOfWork _unitOfWork;

        protected DbSet<TEntity> GetSet()
        {
            return _unitOfWork.CreateSet<TEntity>();
        }

        #region Constructor

        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region IRepository Members

        public IUnitOfWork UnitOfWork => (IUnitOfWork)_unitOfWork;

        public Task<TEntity> GetAsync(object[] keyValues)
        {
            return keyValues != null ? GetSet().FindAsync(keyValues).AsTask() : null;
        }


        public virtual IQueryable<TEntity> GetAll(bool readOnly = true)
        {
            if (readOnly)
                return GetSet().AsNoTracking();
            return GetSet();
        }

        public virtual void Add(TEntity item)
        {
            GetSet().Add(item); // add new item in this set
        }

        public virtual void Delete(TEntity item)
        {
            if (item != null)
            {
                //attach item if not exist
                _unitOfWork.Attach(item);

                //set as "removed"
                GetSet().Remove(item);
            }
        }

        public virtual void TrackItem(TEntity item)
        {
            if (item != null)
                _unitOfWork.Attach(item);
        }

        public virtual void Update(TEntity item)
        {
            //this operation also attach item in object state manager
            _unitOfWork.SetModified(item);
        }

        public virtual void Update(TEntity item, string[] includedProperties)
        {
            //this operation also attach item in object state manager
            _unitOfWork.SetModified(item, includedProperties);
        }

        #endregion
    }
}
