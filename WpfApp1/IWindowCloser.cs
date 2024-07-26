namespace WpfApp1;

public interface IWindowCloser
{
    Action Close { get; set; }
    bool CanClose();

}