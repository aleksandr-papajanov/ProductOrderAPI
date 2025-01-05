using ProductOrderApi.Helpers.Exceptions;
using System.Net;

namespace ProductOrderApi.Entities
{
    internal enum OrderStatus
    {
        Created = 1,
        Confirmed,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Returned,
        Refunded
    }

    internal static class OrderStatusLabels
    {
        private static readonly Dictionary<OrderStatus, string> _statuses = new()
        {
            { OrderStatus.Created, "Created" },
            { OrderStatus.Confirmed, "Confirmed" },
            { OrderStatus.Processing, "Processing" },
            { OrderStatus.Shipped, "Shipped" },
            { OrderStatus.Cancelled, "Cancelled" },
            { OrderStatus.Returned, "Returned" },
            { OrderStatus.Refunded, "Refunded" }
        };

        public static string Parse(OrderStatus statusCode)
        {
            if (_statuses.TryGetValue(statusCode, out var label))
            {
                return label;
            }

            throw new ServiceLayerApiException("Failed to parse order status value", HttpStatusCode.BadRequest);
        }

        public static OrderStatus Parse(string label)
        {
            foreach (var current in _statuses)
            {
                if (current.Value.Equals(label, StringComparison.OrdinalIgnoreCase))
                {
                    return current.Key;
                }
            }

            throw new ServiceLayerApiException("Failed to parse order status string", HttpStatusCode.BadRequest);
        }

        public static string[] GetAll()
        {
            return _statuses.Values.ToArray();
        }
    }
}