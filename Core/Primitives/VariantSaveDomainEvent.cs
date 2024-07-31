namespace Core.Primitives;

public record VariantSaveDomainEvent(Guid Id, VariantId VariantId) : DomainEvent(Id);