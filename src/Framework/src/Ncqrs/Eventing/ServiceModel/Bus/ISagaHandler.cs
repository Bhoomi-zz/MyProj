
namespace Ncqrs.Eventing.ServiceModel.Bus
{
    public interface ISagaHandler<TEvent> where TEvent : IEvent
    {
        void Handle(TEvent evnt);
    }
}
