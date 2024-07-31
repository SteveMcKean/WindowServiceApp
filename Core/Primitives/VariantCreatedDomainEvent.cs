namespace Core.Primitives;

public record VariantCreatedDomainEvent(Guid Id, VariantId VariantId) : DomainEvent(Id);