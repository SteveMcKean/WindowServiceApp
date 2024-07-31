namespace Cpi.Domain.Variants;

public record Sku
{
    private Sku(string value) => Value = value;
    
    public string Value { get; init; }
    public static Sku? Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }
        
        return new Sku(value);
    }
}