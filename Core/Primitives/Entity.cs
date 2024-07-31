namespace Core.Primitives;

public abstract class Entity
{
    private readonly List<DomainEvent> domainEvents = new();
    
    protected void Raise(DomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }
}