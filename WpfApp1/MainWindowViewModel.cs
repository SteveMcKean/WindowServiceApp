using System.Windows.Media;
using Core;

namespace WpfApp1;

public class MainWindowViewModel: BindableBase
{
    private readonly IDialogService dialogService;
    
    public DelegateCommand ShowDialogCommand { get; set; }

    public MainWindowViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
    {
        this.dialogService = dialogService;
        ShowDialogCommand = new DelegateCommand(ExecuteShowDialog);
        
        eventAggregator.GetEvent<MessageSentEvent>().Subscribe(OnNotification);
        
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