namespace WpfApp1;

public class TimeService : ITimeService
{
    public DateTime GetTime()
    {
        return DateTime.UtcNow;
    }
}