namespace WpfApp1;

public interface ICloseWindows
{
    Action? Close { get; set; }
    bool CanClose();

}