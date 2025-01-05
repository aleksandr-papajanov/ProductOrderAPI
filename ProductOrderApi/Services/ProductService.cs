using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using ProductOrderApi.Entities;
using ProductOrderApi.Helpers.Exceptions;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Net;

namespace ProductOrderApi.Services
{
    internal class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Feature> _featureRepository;
        private readonly IRepository<ProductFeature> _productFeatureRepository;

        public IQueryable<Product> Products => _productRepository.All;

        public ProductService(
            IRepository<Product> productRepository,
            IRepository<Feature> featureRepository,
            IRepository<ProductFeature> productFeatureRepository)
        {
            _productRepository = productRepository;
            _featureRepository = featureRepository;
            _productFeatureRepository = productFeatureRepository;
        }

        private async Task EnsureProductUniqAsync(Product product)
        {
            var exists = await _productRepository.All
                .Where(e => e.Code.Equals(product.Code, StringComparison.OrdinalIgnoreCase) && e.Id != product.Id)
                .AnyAsync();

            if (exists) throw new EntityAlreadyExistsApiException($"Product with code {product.Code} already exists");

            exists = await _productRepository.All
                .Where(e => e.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase) && e.Id != product.Id)
                .AnyAsync();

            if (exists) throw new EntityAlreadyExistsApiException($"Product with name {product.Name} already exists");
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            var product = await _productRepository.All
                .Include(e => e.Features)
                    .ThenInclude(e => e.Feature)
                .FirstOrDefaultAsync(e => e.Id == productId);

            if (product == null)
            {
                throw new EntityNotFoundApiException($"Product with id {productId} hasn't been found");
            }

            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            await EnsureProductUniqAsync(product);
            await _productRepository.AddAsync(product);
        }
        
        public async Task UpdateProductAsync(Product product)
        {
            await EnsureProductUniqAsync(product);
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(Product product)
        {
            var features = product.Features.Select(e => e.Feature).ToList();

            await _productFeatureRepository.DeleteRangeAsync(product.Features);
            await _productRepository.DeleteAsync(product);

            foreach (var feature in features)
            {
                await DeleteFeatureIfUnusedAsync(feature);
            }
        }

        private async Task DeleteFeatureIfUnusedAsync(Feature item)
        {
            var isUsed = await _productFeatureRepository.All
                .Where(e => e.FeatureId == item.Id)
                .AnyAsync();

            if (!isUsed)
            {
                await _featureRepository.DeleteAsync(item);
            }
        }
        public async Task UpdateProductStockAsync(Product product, int quantityChange)
        {
            product.QuantityInStock -= quantityChange;

            if (product.QuantityInStock < 0)
            {
                throw new ServiceLayerApiException($"Product with id {product.Id} doesn't have enough quantity in stock.", HttpStatusCode.Conflict);
            }

            await _productRepository.UpdateAsync(product);
        }
        public async Task AddFeaturesAsync(Product product, IDictionary<string, string> features)
        {
            foreach (var current in features)
            {
                var feature = await _featureRepository.All
                    .Where(e => e.Name.Equals(current.Key, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefaultAsync();

                if (feature == null)
                {
                    feature = new Feature
                    {
                        Name = current.Key
                    };

                    await _featureRepository.AddAsync(feature);
                }

                var productFeature = new ProductFeature
                {
                    ProductId = product.Id,
                    FeatureId = feature.Id,
                    Value = current.Value
                };

                await _productFeatureRepository.AddAsync(productFeature);
            }
        }

        private async Task<Feature> GetFeatureAsync(string name)
        {
            var feature = await _featureRepository.All
                .Where(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync();

            if (feature == null)
            {
                throw new EntityNotFoundApiException($"Product feature specified by name '{name}' hasn't been found");
            }

            return feature;
        }

        public async Task DeleteFeaturesAsync(Product product, IList<string> toDelete)
        {
            if (toDelete.Count == 0) return;

            var productFeatures = product.Features
                .Where(e => toDelete.Contains(e.Feature.Name))
                .ToList();

            var features = productFeatures
                .Select(e => e.Feature)
                .ToList();

            await _productFeatureRepository.DeleteRangeAsync(productFeatures);

            foreach (var feature in features)
            {
                await DeleteFeatureIfUnusedAsync(feature);
            }
        }

        public async Task UpdateFeaturesAsync(Product product, IDictionary<string, string> features)
        {
            if (features.Count == 0) return;

            foreach (var productFeature in product.Features)
            {
                var featureName = productFeature.Feature.Name;

                if (features.ContainsKey(featureName))
                {
                    productFeature.Value = features[featureName];
                    await _productFeatureRepository.UpdateAsync(productFeature);
                }
            }
        }
    }
}
