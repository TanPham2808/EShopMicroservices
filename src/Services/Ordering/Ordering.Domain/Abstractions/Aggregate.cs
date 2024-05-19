
namespace Ordering.Domain.Abstractions;

public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Một phương thức để xóa và trả về các sự kiện miền hiện tại trong Aggregate
    /// </summary>
    /// <returns></returns>
    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] dequeuedEvents = _domainEvents.ToArray();

        _domainEvents.Clear();

        return dequeuedEvents;
    }
}

