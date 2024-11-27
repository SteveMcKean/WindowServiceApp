namespace WpfApp1;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TargetTypeAttribute(Type targetType) : Attribute
{
    public Type TargetType { get; } = targetType;
}
