namespace WpfApp1;

public class NotificationViewModel: BindableBase
{
    private readonly ITimeService timeService;
    private string currentTime;

    public string CurrentTime
    {
        get => currentTime;
        set => SetProperty(ref currentTime, value);
    }

    public NotificationViewModel(ITimeService timeService)
    {
        this.timeService = timeService;
        
        CurrentTime = timeService.GetTime().ToString();
    }
    
}