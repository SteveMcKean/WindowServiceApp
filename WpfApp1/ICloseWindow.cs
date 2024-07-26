namespace WpfApp1;

public interface ICloseWindow
{
    Action Close { get; set; }
    bool CanClose();

}