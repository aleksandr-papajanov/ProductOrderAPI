using Microsoft.EntityFrameworkCore;
using ProductOrderApi.Entities;
using ProductOrderApi.Infrastructure.Interfaces;
using System;

namespace ProductOrderApi.Infrastructure
{
    internal class Repository<T> : IRepository<T>
        where T : EntityBase
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _set;

        public virtual IQueryable<T> All => _set.AsQueryable();

        public Repository(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();

        }

        public virtual async Task AddAsync(T item)
        {
            await _set.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T item)
        {
            _set.Remove(item);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public virtual async Task UpdateAsync(T item)
        {
            _set.Update(item);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> range)
        {
            _set.RemoveRange(range);
            await _context.SaveChangesAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> item)
        {
            _set.AddRange(item);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> range)
        {
            _set.UpdateRange(range);
            await _context.SaveChangesAsync();
        }

        public virtual async Task SaveAsync(T item)
        {
            await _context.SaveChangesAsync();
        }

        public virtual async Task ReloadAsync(T item)
        {
            await _context.Entry(item).ReloadAsync();
        }
    }
}
