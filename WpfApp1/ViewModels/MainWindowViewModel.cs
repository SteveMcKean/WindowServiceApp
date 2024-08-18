using Core;

namespace WpfApp1.ViewModels;

public class MainWindowViewModel: BindableBase, IWindowCloser
{
    private readonly IDialogService dialogService;
    
    public DelegateCommand ShowDialogCommand { get; set; }
    public DelegateCommand CloseWindowCommand { get; set; }
    
    public CpiSkuDimensionVariant InputDimVar { get; set; }
    public Action Close { get; set; }
    public string Sku = "1234";
    
    public MainWindowViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
    {
        this.dialogService = dialogService;
        ShowDialogCommand = new DelegateCommand(ExecuteShowDialog);
        CloseWindowCommand = new DelegateCommand(OncloseWindow);
        
        eventAggregator.GetEvent<MessageSentEvent>().Subscribe(OnNotification);
        InputDimVar = new CpiSkuDimensionVariant();
        
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
        // dialogService.ShowDialog<NotificationViewModel, bool>((result, viewModel) =>
        //     {
        //         if (result)
        //         {
        //             InputDimVar.OverrideAspectRatio = viewModel.SelectedCpiSkuDimensionVariant.OverrideAspectRatio;
        //         }
        //
        //         return result;
        //     }, new { SelectedCpiSkuDimensionVariant = InputDimVar, SelectedSku = Sku });
        //
        dialogService.ShowDialog("TestDialogView", result =>
            {
                var test = result;
                
            });
        
        // dialogService.ShowDialog<NotificationViewModel>(result =>
        //     {
        //         var test = result;
        //         
        //     }, new { SelectedCpiSkuDimensionVariant = InputDimVar, SelectedSku = Sku });
    }

    
}