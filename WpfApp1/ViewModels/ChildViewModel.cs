namespace WpfApp1.ViewModels;

public class ChildViewModel: BindableBase,IWindowCloser
{
    public Action? Close { get; set; }

    public DelegateCommand CloseWindowCommand { get; set; }

    public ChildViewModel()
    {
        CloseWindowCommand = new DelegateCommand(OnCloseWindow);
    }

    public bool CanClose()
    {
        return true;
    }

    private void OnCloseWindow()
    {
        Close?.Invoke();

    }
}