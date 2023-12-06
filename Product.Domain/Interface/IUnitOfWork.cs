using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Product.Domain.Interface
{
    public interface IUnitOfWork
        : IDisposable
    {
        string ConnectionString { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        int Commit();
        Task<int> CommitAsync();
        void DetachAllEntities();
    }
}
