using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductOrderApi.DTOs.Orders;
using ProductOrderApi.Entities;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Commands.Orders
{
    internal class GetAllOrdersCommand : IRequest<IEnumerable<Order>>, IUserDependent
    {
        public GetAllOrdersRequest Request { get; private set; }

        public User CurrentUser { get; set; } = null!;

        public GetAllOrdersCommand(GetAllOrdersRequest request)
        {
            Request = request;
        }
    }

    internal class GetAllOrdersCommandHandler : IRequestHandler<GetAllOrdersCommand, IEnumerable<Order>>
    {
        private readonly IOrderService _service;

        public GetAllOrdersCommandHandler(IOrderService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<Order>> Handle(GetAllOrdersCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var query = _service.Orders
                .Include(e => e.Cart)
                    .ThenInclude(e => e.Product)
                .Include(e => e.Tracking.OrderByDescending(e => e.UpdatedAt))
                .AsQueryable();

            if (!command.CurrentUser.IsInRole("Admin"))
            {
                query = query.Where(e => e.UserId == command.CurrentUser.Id);
            }

            query = ApplyFilter(query, request.FilterBy, request.Filter);
            query = ApplySorting(query, request.OrderBy, request.OrderDirection);
            query = ApplyLimit(query, request.Skip, request.Take);

            return await query.ToListAsync();
        }

        private IQueryable<Order> ApplyLimit(IQueryable<Order> query, int skip, int take)
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

        private IQueryable<Order> ApplySorting(IQueryable<Order> query, string? orderBy, string orderDirection)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return query;
            }

            switch (orderBy)
            {
                case "TotalPrice":
                    query = orderDirection == "ASC"
                        ? query.OrderBy(e => e.TotalPrice)
                        : query.OrderByDescending(e => e.TotalPrice);
                    break;
                case "Status":
                    query = orderDirection == "ASC"
                        ? query.OrderBy(e => e.Tracking.OrderByDescending(t => t.UpdatedAt).First().Status)
                        : query.OrderByDescending(e => e.Tracking.OrderByDescending(t => t.UpdatedAt).First().Status);
                    break;
            }

            return query;
        }

        private IQueryable<Order> ApplyFilter(IQueryable<Order> query, string? filter, string? filterBy)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return query;
            }

            switch (filterBy)
            {
                case "ProductName":
                    query = query.Where(e => e.Cart.Any(a => a.Product.Name.Contains(filter)));
                    break;
                case "ProductCode":
                    query = query.Where(e => e.Cart.Any(a => a.Product.Code.Contains(filter)));
                    break;
                case "Status":
                    query = query.Where(e => e.Tracking.OrderByDescending(t => t.UpdatedAt).First().Status == OrderStatusLabels.Parse(filter));
                    break;
                case "Comment":
                    query = query.Where(e => e.Comment != null && e.Comment.Contains(filter));
                    break;
            }

            return query;
        }
    }
}
