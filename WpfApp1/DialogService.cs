using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using DialogWindow = WpfApp1.Views.DialogWindow;

namespace WpfApp1;

public class DialogService : IDialogService
{
    private static readonly Dictionary<Type,Type> Mappings = new();


    public static IServiceProvider? ServiceProviderFactory { get; set; }

    public static void RegisterDialog<TView, TViewModel>()
    {
        Mappings[typeof(TViewModel)] = typeof(TView);
        
    }
    
    public void ShowDialog(string name, Action<bool> callBack)
    {
        var type = Type.GetType($"WpfApp1.Views.{name}");
        if (type != null)
        {
            ShowDialogInternal(type, callBack, null);
        }
    }
    
    public void ShowDialog<TViewModel>(Action<bool> callBack)
    {
        var type = Mappings[typeof(TViewModel)];
        ShowDialogInternal(type, callBack, typeof(TViewModel));
    }

    public void ShowDialog<TViewModel, TResult>(Func<TResult, TViewModel, bool> callBack, object? parameters = null) 
        where TViewModel : class, new()
    {
        if (!Mappings.TryGetValue(typeof(TViewModel), out var viewType))
        {
            throw new InvalidOperationException($"No view registered for ViewModel type {typeof(TViewModel).FullName}");
        }
        
        ShowDialogInternal(viewType, (viewModel) =>
            {
                var vm = (TViewModel)viewModel;

                // Pass the parameters to the ViewModel, if any
                if (parameters != null)
                {
                    foreach (var prop in parameters.GetType().GetProperties())
                    {
                        var propInViewModel = vm.GetType().GetProperty(prop.Name);
                        if (propInViewModel != null && propInViewModel.CanWrite)
                        {
                            propInViewModel.SetValue(vm, prop.GetValue(parameters));
                        }
                    }
                }

                // Show the dialog and get the result
                var dialogResult = ShowDialogAndGetResult<TResult, TViewModel>(vm);

                // Invoke the callback with the result and the ViewModel
                callBack(dialogResult, vm);

            }, typeof(TViewModel));
    }

    public void ShowDialog<TViewModel>(Action<bool> callBack, object parameters = null)
    {
        var type = Mappings[typeof(TViewModel)];
        ShowDialogInternal(type, callBack, typeof(TViewModel), parameters);
    }

    private static TResult ShowDialogAndGetResult<TResult, TViewModel>(TViewModel viewModel)
    {
        // Assuming DialogWindow is your base dialog window
        var dialog = new DialogWindow
            {
                DataContext = viewModel
            };

        // Show the dialog modally and wait for it to close
        dialog.ShowDialog();

        // You need to determine how to convert the dialog's result to TResult
        // For demonstration, assume the dialog sets a property on the ViewModel
        // that can be used as the TResult

        // Example: Assuming the ViewModel has a property of type TResult
        var resultProperty = typeof(TViewModel).GetProperty("DialogResult");
        if (resultProperty != null && resultProperty.PropertyType == typeof(TResult))
        {
            return (TResult)resultProperty.GetValue(viewModel);
        }

        // If the dialog does not set a result directly, you may need to handle this differently.
        return default;
    }
    
    private static void ShowDialogInternal(Type viewType, Action<object> setupViewModel, Type? vmType)
    {
        EventHandler? closeHandler = null;

        var dialog = new DialogWindow();
        var content = (UserControl?)Activator.CreateInstance(viewType);

        object viewModel = null;

        if (vmType != null)
        {
            viewModel = ServiceProviderFactory!.GetRequiredService(vmType);
            if (content != null)
            {
                content.DataContext = viewModel;
            }
        }

        dialog.Content = content;

        closeHandler = (sender, args) =>
            {
                dialog.Closed -= closeHandler;
                // Optionally, if TResult is a specific type, you might fetch the result from the ViewModel or dialog.
            };

        dialog.Closed += closeHandler;
        setupViewModel?.Invoke(viewModel); // Pass the ViewModel to the setup function

        dialog.ShowDialog();
    }
    
    private static void ShowDialogInternal(Type type, Action<bool> callBack, Type? vmType, object parameters = null)
    {
        EventHandler? closeHandler = null;

        var dialog = new DialogWindow();
        var content = Activator.CreateInstance(type) as UserControl;

        if (content == null)
        {
            throw new InvalidOperationException($"The type {type.FullName} is not a UserControl.");
        }

        if (vmType != null)
        {
            var viewModel = ServiceProviderFactory!.GetRequiredService(vmType);
            if (parameters != null)
            {
                foreach (var prop in parameters.GetType().GetProperties())
                {
                    var propInViewModel = viewModel.GetType().GetProperty(prop.Name);
                    if (propInViewModel != null && propInViewModel.CanWrite)
                    {
                        propInViewModel.SetValue(viewModel, prop.GetValue(parameters));
                    }
                }
            }

            content.DataContext = viewModel;
        }

        dialog.Content = content;
        closeHandler = (sender, args) =>
        {
            dialog.Closed -= closeHandler;
            if (dialog.DialogResult != null)
            {
                callBack?.Invoke(dialog.DialogResult.Value);
            }
        };

        dialog.Closed += closeHandler;
        dialog.ShowDialog();
    }
   
}