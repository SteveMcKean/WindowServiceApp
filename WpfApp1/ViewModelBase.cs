using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    private readonly Dictionary<Type, object> services = new();

    protected void RegisterService<T>(T service) where T : class
    {
        services[typeof(T)] = service;
    }

    protected T GetService<T>() where T : class
    {
        services.TryGetValue(typeof(T), out var service);
        return service as T;
    }

    // Implement INotifyPropertyChanged here if needed
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
