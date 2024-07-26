using System.Windows;

namespace WpfApp1;

public static class ViewModelLocator
{
    public static IServiceProvider ServiceProvider { get; set; }

    public static bool GetAutoWireViewModel(DependencyObject obj)
    {
        return (bool)obj.GetValue(AutoWireViewModelProperty);
    }

    public static void SetAutoWireViewModel(DependencyObject obj, bool value)
    {
        obj.SetValue(AutoWireViewModelProperty, value);
    }

    public static readonly DependencyProperty AutoWireViewModelProperty =
        DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), 
            new PropertyMetadata(false, AutoWireViewModelChanged));

    private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue != null && (bool)e.NewValue)
        {
            Bind(d);
        }
    }

    private static void Bind(DependencyObject view)
    {
        var viewType = view.GetType();
        var viewName = viewType.FullName;
        
        var viewAssemblyName = viewType.Assembly.FullName;
        var viewModelName = $"{viewName}ViewModel, {viewAssemblyName}";
        
        viewModelName = viewModelName.Replace("Views", "ViewModels");
        
        var viewModelType = Type.GetType(viewModelName);
        if (viewModelType == null)
        {
            throw new InvalidOperationException($"Cannot locate view model type for {viewModelName}.");
        }

        var viewModel = ServiceProvider.GetService(viewModelType);
        ((FrameworkElement)view).DataContext = viewModel;
        
    }
}
