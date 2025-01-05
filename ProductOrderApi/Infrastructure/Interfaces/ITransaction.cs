using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace ProductOrderApi.Infrastructure.Interfaces
{
    internal interface ITransaction
    {
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel level);
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
