using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Views;
using DialogWindow = WpfApp1.Views.DialogWindow;

namespace WpfApp1;

public class DialogService : IDialogService
{
    private static Dictionary<Type,Type> mappings = new Dictionary<Type, Type>();

    public static IServiceProvider? ServiceProvider { get; set; }

    public static void RegisterDialog<TView, TViewModel>()
    {
        mappings[typeof(TViewModel)] = typeof(TView);
        
    }
    
    public void ShowDialog(string name, Action<string> callBack)
    {
        var type = Type.GetType($"WpfApp1.{name}");
        if (type != null)
        {
            ShowDialogInternal(type, callBack, null);
        }
    }
    
    public void ShowDialog<TViewModel>(Action<string> callBack)
    {
        var type = mappings[typeof(TViewModel)];
        ShowDialogInternal(type, callBack, typeof(TViewModel));
        
    }

    private static void ShowDialogInternal(Type type, Action<string> callBack, Type? vmType)
    {
        EventHandler? closeHandler = null;

        var dialog = new DialogWindow();
        var content = (UserControl?)Activator.CreateInstance(type);

        if (vmType != null)
        {
            var viewModel = ServiceProvider!.GetRequiredService(vmType);
            if (content != null)
            {
                content.DataContext = viewModel;
            }
        }

        dialog.Content = content;
        
        closeHandler = (sender, args) =>
            {
                dialog.Closed -= closeHandler;
                if (dialog.DialogResult != null)
                {
                    callBack?.Invoke(dialog.DialogResult.ToString());
                }
            };    
        
        dialog.Closed += closeHandler;
        dialog.ShowDialog();
        
    }
   
}