namespace Cpi.Domain.Variants;

public class InboundDimensionVariant
{
    private readonly HashSet<InboundDimensionVariant> children = new();
    public VariantId Id { get; private set; }
    public VariantId ParentId { get; set; }
    public int Length { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Weight { get; private set; }
    
    private InboundDimensionVariant(VariantId id, int length, int width, int height, int weight)
    {
        Id = id;
        Length = length;
        Width = width;
        Height = height;
        Weight = weight;
    }
    
    public static InboundDimensionVariant Create(VariantId id, int length, int width, int height, int weight)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }
        
        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }
        
        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }
        
        if (weight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight));
        }
        
        return new InboundDimensionVariant(id, length, width, height, weight);
        
    }
    
    public void AddVariant(InboundDimensionVariant child)
    {
        children.Add(child);
    }
    
    public void RemoveVariant(InboundDimensionVariant child)
    {
        children.Remove(child);
    }
}