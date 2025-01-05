using MediatR;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Data;

namespace ProductOrderApi.Commands.Users
{
    internal class ConfirmEmailCommand : IRequest, ITransactionDependent
    {
        public Guid Token { get; private set; }

        public IsolationLevel IsolationLevel => IsolationLevel.Serializable;

        public ConfirmEmailCommand(Guid token)
        {
            Token = token;
        }
    }

    internal class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly IUserService _service;

        public ConfirmEmailCommandHandler(IUserService service)
        {
            _service = service;
        }

        public async Task Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
        {
            await _service.ConfirmEmail(command.Token);
        }
    }
}
