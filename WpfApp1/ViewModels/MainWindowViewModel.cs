using Core;

namespace WpfApp1.ViewModels;

public class MainWindowViewModel: BindableBase, IWindowCloser
{
    private readonly IDialogService dialogService;
    
    public DelegateCommand ShowDialogCommand { get; set; }
    public DelegateCommand CloseWindowCommand { get; set; }
    
    public Action Close { get; set; }
    
    public MainWindowViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
    {
        this.dialogService = dialogService;
        ShowDialogCommand = new DelegateCommand(ExecuteShowDialog);
        CloseWindowCommand = new DelegateCommand(OncloseWindow);
        
        eventAggregator.GetEvent<MessageSentEvent>().Subscribe(OnNotification);
        
    }

    public bool CanClose()
    {
        return true;
    }
    
    private void OncloseWindow()
    {
        Close?.Invoke();
        
    }

    private void OnNotification(string message)
    {
        
    }

    private void ExecuteShowDialog()
    {
        
        dialogService.ShowDialog<NotificationViewModel>(result =>
            {
                var test = result;
                
            });
    }

    
}