using System.DirectoryServices;

namespace WpfApp1;

public interface IDialogService
{
    void ShowDialog(string name, Action<string> callBack);
    void ShowDialog<TViewModel>(Action<string> callBack);
}   