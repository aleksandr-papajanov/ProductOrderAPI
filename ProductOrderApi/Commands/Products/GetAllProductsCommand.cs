using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductOrderApi.DTOs.Products;
using ProductOrderApi.Entities;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Commands.Products
{
    internal class GetAllProductCommand : IRequest<IEnumerable<Product>>
    {
        public GetAllProductsRequest Request { get; private set; }

        public GetAllProductCommand(GetAllProductsRequest request)
        {
            Request = request;
        }
    }

    internal class GetAllProductCommandHandler : IRequestHandler<GetAllProductCommand, IEnumerable<Product>>
    {
        private readonly IProductService _service;

        public GetAllProductCommandHandler(IProductService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var query = _service.Products
                .Include(e => e.Features)
                    .ThenInclude(e => e.Feature)
                .AsQueryable();

            query = ApplyFilter(query, request.FilterBy, request.Filter);
            query = ApplySorting(query, request.OrderBy, request.OrderDirection);
            query = ApplyLimit(query, request.Skip, request.Take);

            return await query.ToListAsync();
        }

        private IQueryable<Product> ApplyLimit(IQueryable<Product> query, int skip, int take)
        {
            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
            {
                query = query.Take(take);
            }

            return query;
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string? orderBy, string orderDirection)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return query;
            }

            switch (orderBy)
            {
                case "Id":
                    query = orderDirection == "ASC"
                        ? query.OrderBy(e => e.Id)
                        : query.OrderByDescending(e => e.Id);
                    break;
                case "Name":
                    query = orderDirection == "ASC"
                        ? query.OrderBy(e => e.Name)
                        : query.OrderByDescending(e => e.Name);
                    break;
                case "Code":
                    query = orderDirection == "ASC"
                        ? query.OrderBy(e => e.Code)
                        : query.OrderByDescending(e => e.Code);
                    break;
                case "Price":
                    query = orderDirection == "ASC"
                        ? query.OrderBy(e => e.Price)
                        : query.OrderByDescending(e => e.Price);
                    break;
                case "CreatedAt":
                    query = orderDirection == "ASC"
                        ? query.OrderBy(e => e.CreatedAt)
                        : query.OrderByDescending(e => e.CreatedAt);
                    break;
                case "ModifiedAt":
                    query = orderDirection == "ASC"
                        ? query.OrderBy(e => e.ModifiedAt)
                        : query.OrderByDescending(e => e.ModifiedAt);
                    break;
            }

            return query;
        }

        private IQueryable<Product> ApplyFilter(IQueryable<Product> query, string? filterBy, string? filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return query;
            }

            switch (filterBy)
            {
                case "Features":
                    query = query.Where(e => e.Features.Any(a => a.Value.Contains(filter)));
                    break;
                case "Name":
                    query = query.Where(e => e.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));
                    break;
                case "Code":
                    query = query.Where(e => e.Code.Contains(filter, StringComparison.OrdinalIgnoreCase));
                    break;
                case "Description":
                    query = query.Where(e => e.Description != null && e.Description.Contains(filter, StringComparison.OrdinalIgnoreCase));
                    break;
            }

            return query;
        }
    }
}