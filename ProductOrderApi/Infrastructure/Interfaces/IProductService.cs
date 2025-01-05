using ProductOrderApi.Entities;

namespace ProductOrderApi.Infrastructure.Interfaces
{
    internal interface IProductService
    {
        IQueryable<Product> Products { get; }

        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<Product> GetProductAsync(int productId);
        Task UpdateProductStockAsync(Product product, int quantityChange);
        Task AddFeaturesAsync(Product product, IDictionary<string, string> features);
        Task UpdateFeaturesAsync(Product product, IDictionary<string, string> features);
        Task DeleteFeaturesAsync(Product product, IList<string> features);
    }
}
