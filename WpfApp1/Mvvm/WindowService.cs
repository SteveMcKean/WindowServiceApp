using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Mvvm;

[TargetType(typeof(UserControl))]
[TargetType(typeof(Window))]
public class WindowService: IWindowService, IDocumentOwner
{
    public void Show<TView>(object viewModel) where TView : Window, new()
    {
        throw new NotImplementedException();
    }

    public bool? ShowDialog<TView>(object viewModel) where TView : Window, new()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void DestroyDocument(object document)
    {
        throw new NotImplementedException();
    }
}