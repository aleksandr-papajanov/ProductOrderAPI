using ProductOrderApi.Entities;

namespace ProductOrderApi.Infrastructure.Interfaces
{
    internal interface IRepository<T>
        where T : EntityBase
    {
        IQueryable<T> All { get; }
        Task<T?> GetAsync(int id);
        Task AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> range);
        Task UpdateAsync(T item);
        Task UpdateRangeAsync(IEnumerable<T> range);
        Task DeleteAsync(T item);
        Task DeleteRangeAsync(IEnumerable<T> range);
        Task ReloadAsync(T item);
        Task SaveAsync(T item);
    }
}
