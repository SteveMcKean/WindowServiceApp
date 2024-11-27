using Core;
using WpfApp1.Behaviors;
using WpfApp1.Views;

namespace WpfApp1.ViewModels;

public class MainWindowViewModel : BindableBase, IWindowCloser
{
    private readonly IDialogService dialogService;

    public DelegateCommand<object> OpenDialogCommand { get; set; }

    public DelegateCommand CloseWindowCommand { get; set; }
    public DelegateCommand ShowNotificationCommand { get; set; }

    public CpiSkuDimensionVariant InputDimVar { get; set; }

    public Action? Close { get; set; }

    public string Sku { get; set; } = "123456";

    public MainWindowViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
    {
        this.dialogService = dialogService;

        OpenDialogCommand = new DelegateCommand<object>(ExecuteShowDialog);
        CloseWindowCommand = new DelegateCommand(OnCloseWindow);
        ShowNotificationCommand = new DelegateCommand(ShowNotification);

        eventAggregator.GetEvent<MessageSentEvent>().Subscribe(OnNotification);
        InputDimVar = new CpiSkuDimensionVariant();

    }

    public bool CanClose()
    {
        return true;
    }

    private void OnCloseWindow()
    {
        Close?.Invoke();

    }

    private void OnNotification(string message)
    {

    }

    private static void ExecuteShowDialog(object parameter)
    {
        if (parameter is WindowServiceBehavior windowService)
        {
            windowService.ShowDialog(new ChildViewModel());
        }
    }

    private void ShowNotification()
    {
        dialogService.ShowDialog<NotificationViewModel>(result =>
            {
                var test = result;
                
            }, new { SelectedCpiSkuDimensionVariant = InputDimVar, SelectedSku = Sku });


       // dialogService.ShowDialog<NotificationViewModel, DialogResult>((b, model) => model.DialogResult);

    }

}