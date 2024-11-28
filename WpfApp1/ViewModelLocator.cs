using System.Windows;

namespace WpfApp1;

public static class ViewModelLocator
{
    public static IServiceProvider ServiceProviderFactory { get; set; } = null!;

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

        if (viewName == null)
            throw new InvalidOperationException("The view name could not be determined.");

        // Assuming views are in the "Views" namespace and view models in the "ViewModels" namespace
        var viewAssemblyName = viewType.Assembly.FullName;
        var viewModelName = viewName.Replace(".Views.", ".ViewModels.") + "Model";

        // Attempt to locate the view model type
        var viewModelType = Type.GetType($"{viewModelName}, {viewAssemblyName}");

        if (viewModelType == null)
        {
            throw new InvalidOperationException($"Cannot locate view model type for {viewModelName}.");
        }

        // Resolve the view model instance from the service provider
        var viewModel = ServiceProviderFactory.GetService(viewModelType);

        if (viewModel == null)
        {
            throw new InvalidOperationException($"Unable to resolve view model type {viewModelType} from service provider.");
        }

        // Set the DataContext of the view
        ((FrameworkElement)view).DataContext = viewModel;
    }
}
