using System.Dynamic;
using System.Text;

namespace Core;

public class CustomDynamic: DynamicObject
{
    private readonly Dictionary<string, object> properties = new();
    
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        var name = binder.Name.ToLower();
        return properties.TryGetValue(name, out result);
    }
    
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        properties[binder.Name.ToLower()] = value;
        return true;
    }
    
    public override IEnumerable<string> GetDynamicMemberNames()
    {
        return properties.Keys;
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var (key, value) in properties)
        {
            sb.Append($"{key}: {value}, ");
        }
     
        return sb.ToString();
    }
    
    public object this[string key]
    {
        get => properties[key];
        set => properties[key] = value;
    }
}