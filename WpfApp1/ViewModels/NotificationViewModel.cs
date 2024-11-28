using Core;

namespace WpfApp1.ViewModels;

public class NotificationViewModel: BindableBase, ICloseWindows
{
    private readonly ITimeService timeService;
    private string currentTime;
    private readonly CustomDynamic dynamic;

    public bool DialogResult { get; set; }
    public DelegateCommand CloseWindowCommand { get; set; }

    public CpiSkuDimensionVariant SelectedCpiSkuDimensionVariant { get; set; }
    public string SelectedSku { get; set; }
    
    public string CurrentTime
    {
        get => currentTime;
        set => SetProperty(ref currentTime, value);
    }
    
    public Action? Close { get; set; }

    public NotificationViewModel()
    {
        CloseWindowCommand = new DelegateCommand(OnCloseWindow);
    }

    public void OnCloseWindow()
    {
        Close?.Invoke();
    }

    public bool CanClose()
    {
        return true;
    }
}