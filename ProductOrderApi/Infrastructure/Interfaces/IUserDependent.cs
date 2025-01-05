using ProductOrderApi.Entities;

namespace ProductOrderApi.Infrastructure.Interfaces
{
    internal interface IUserDependent
    {
        User CurrentUser { get; set; }
    }
}
