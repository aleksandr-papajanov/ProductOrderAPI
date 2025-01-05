using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProductOrderApi.Infrastructure;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Data;

namespace ProductOrderApi.Helpers
{
    internal class TransactionHandler : ITransaction
    {
        private readonly AppDbContext _context;

        public TransactionHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel level = IsolationLevel.ReadUncommitted)
        {
            return await _context.Database.BeginTransactionAsync(level);
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
