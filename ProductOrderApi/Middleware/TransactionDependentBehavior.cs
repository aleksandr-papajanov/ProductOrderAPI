using MediatR;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Middleware
{
    internal class TransactionDependentBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ITransaction _context;

        public TransactionDependentBehavior(ITransaction context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ITransactionDependent command)
            {
                using var transaction = await _context.BeginTransactionAsync(command.IsolationLevel);

                try
                {
                    var response = await next();

                    await transaction.CommitAsync();

                    return response;
                }
                catch
                {
                    await transaction.RollbackAsync();

                    throw;
                }
            }

            return await next();
        }
    }
}
