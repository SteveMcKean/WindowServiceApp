using System.DirectoryServices;

namespace WpfApp1;

public interface IDialogService
{
    void ShowDialog(string name, Action<bool> callBack);
    void ShowDialog<TViewModel>(Action<bool> callBack);
    void ShowDialog<TViewModel, TResult>(Func<TResult, TViewModel, bool> callBack, object? parameters = null)
        where TViewModel : class, new();
    void ShowDialog<TViewModel>(Action<bool> callBack, object parameters = null);
}   