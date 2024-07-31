using Core.Primitives;

namespace Core.Variants;

public class Variant: Entity
{
    private readonly List<Variant> childVariants = new();
    
    private Variant()
    {
        
    }
    
    public VariantId Id { get; private init; }
    
    public static Variant Create(VariantId id)
    {
        var variant = new Variant
            {
                Id = id
            };

        variant.Raise(new VariantCreatedDomainEvent(Guid.NewGuid(), variant.Id));
        return variant;
        
    }
    
    public void Add(Variant childVariant)
    {
        childVariants.Add(childVariant);
    }
    
    public void Remove(Variant childVariant)
    {
        childVariants.Remove(childVariant);
        Raise(new VariantChildRemovedDomainEvent(Guid.NewGuid(), Id, childVariant.Id));
    }
}