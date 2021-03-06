using System.Threading.Tasks;
using JPProject.Domain.Core.Commands;
using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.Notifications;
using MediatR;

namespace JPProject.Domain.Core.Bus
{
    public sealed class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public InMemoryBus(IMediator mediator, IEventStore eventStore)
        {
            _mediator = mediator;
            _eventStore = eventStore;
        }

        public Task<bool> SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send<bool>(command);
        }

        public async Task RaiseEvent<T>(T @event) where T : Event
        {
            if (!@event.MessageType.Equals(nameof(DomainNotification)))
                await _eventStore.Save(@event);

            await _mediator.Publish(@event);
        }
    }
}