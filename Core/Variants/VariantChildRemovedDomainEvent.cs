using Core.Primitives;

namespace Core.Variants;

public record VariantChildRemovedDomainEvent(Guid Id, VariantId ParentId, VariantId ChildVariantId) : DomainEvent(Id);