using ProductOrderApi.DTOs.Products;
using ProductOrderApi.Entities;

namespace ProductOrderApi.DTOs.Mappers
{
    internal static class ProductMapper
    {
        public static ProductDTO ToDTO(this Product entity)
        {
            return new ProductDTO
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                QuantityInStock = entity.QuantityInStock,
                CreatedAt = entity.CreatedAt,
                ModifiedAt = entity.ModifiedAt,
                IsAvailable = entity.IsAvailable,
                Features = entity.Features.ToDictionary(a => a.Feature.Name, a => a.Value),
            };
        }

        public static Product ToEntity(this ProductDTO dto)
        {
            return new Product
            {
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                QuantityInStock = dto.QuantityInStock,
                IsAvailable = dto.IsAvailable,
            };
        }

        public static ProductInCartDTO ToCartDTO(this Product entity)
        {
            return new ProductInCartDTO
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name
            };
        }

        public static Product ToEntity(this CreateProductRequest request)
        {
            return new Product
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                QuantityInStock = request.QuantityInStock,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                IsAvailable = request.IsAvailable
            };
        }
    }
}
