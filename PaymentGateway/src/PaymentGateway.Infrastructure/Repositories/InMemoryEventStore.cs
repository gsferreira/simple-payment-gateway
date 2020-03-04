using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Infrastructure.Repositories
{
    // Based on: https://github.com/gregoryyoung/m-r/blob/master/SimpleCQRS/EventStore.cs
    public class InMemoryEventStore<T> : IEventStoreRepository<T>
        where T : IEventSourcedEntity
    {
        private readonly IDeferredEventDispatcher _dispatcher;
        private readonly Dictionary<Guid, List<EventDescriptor>> _current = new Dictionary<Guid, List<EventDescriptor>>();

        public InMemoryEventStore(IDeferredEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public Task<T> Get(Guid aggregateId)
        {
            var aggregateEvents = GetEventsForAggregate(aggregateId);
            return Task.FromResult(CreateAggregate(aggregateEvents));
        }

        public async Task Save(T data, int? expectedVersion = null)
        {
            var aggregateId = data.Id;

            // try to get event descriptors list for given aggregate id
            // otherwise -> create empty dictionary
            if (!_current.TryGetValue(aggregateId, out var eventDescriptors))
            {
                eventDescriptors = new List<EventDescriptor>();
                _current.Add(aggregateId, eventDescriptors);
            }
            // check whether latest event version matches current aggregate version
            // otherwise -> throw exception
            else if (expectedVersion.HasValue && eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion.Value)
            {
                throw new InvalidOperationException("Concurrency exception");
            }
            var i = expectedVersion ?? 0;

            var events = data.GetUncommittedChanges();
            // iterate through current aggregate events increasing version with each processed event
            foreach (var @event in events)
            {
                i++;
                @event.Version = i;

                // push event to the event descriptors list for current aggregate
                eventDescriptors.Add(new EventDescriptor(aggregateId, @event, i));

            }
            // publish current event to the bus for further processing by subscribers
            await _dispatcher.Dispatch(events);

            data.MarkChangesAsCommitted();
        }

        // collect all processed events for given aggregate and return them as a list
        // used to build up an aggregate from its history (Domain.LoadsFromHistory)
        private List<EntityEvent> GetEventsForAggregate(Guid aggregateId)
        {
            if (!_current.TryGetValue(aggregateId, out var eventDescriptors))
            {
                throw new InvalidOperationException("Aggregate not found");
            }

            return eventDescriptors.Select(desc => desc.EventData).ToList();
        }

        private T CreateAggregate(IEnumerable<EntityEvent> events)
        {
            var constructor = typeof(T).GetConstructor(new[] { typeof(IEnumerable<EntityEvent>) });
            if (constructor == null)
            {
                throw new InvalidCastException($"Type {typeof(T)} must have a constructor with the following signature: .ctor(IEnumerable<EntityEvent>)");
            }

            return (T)constructor.Invoke(new object[] { events });
        }
        private struct EventDescriptor
        {

            public readonly EntityEvent EventData;
            public readonly Guid Id;
            public readonly int Version;

            public EventDescriptor(Guid id, EntityEvent eventData, int version)
            {
                EventData = eventData;
                Version = version;
                Id = id;
            }
        }
    }

}

