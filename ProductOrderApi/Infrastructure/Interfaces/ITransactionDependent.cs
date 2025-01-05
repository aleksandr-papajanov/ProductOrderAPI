using System.Data;

namespace ProductOrderApi.Infrastructure.Interfaces
{
    internal interface ITransactionDependent
    {
        IsolationLevel IsolationLevel { get; }
    }
}
