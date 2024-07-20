using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace WpfApp1;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
       
        ServiceProvider = serviceCollection.BuildServiceProvider();
        
        ViewModelLocator.ServiceProvider = ServiceProvider;
        DialogService.ServiceProvider = ServiceProvider;
        
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        
    }

    private void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDialogService, DialogService>();
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddSingleton<ITimeService, TimeService>();
        serviceCollection.AddTransient<NotificationViewModel>();
        serviceCollection.AddSingleton<IEventAggregator, EventAggregator>();
        
        DialogService.RegisterDialog<Notification, NotificationViewModel>();
       
    }
}