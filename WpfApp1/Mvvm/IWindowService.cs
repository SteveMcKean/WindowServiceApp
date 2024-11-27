using System.Windows;

namespace WpfApp1.Mvvm;

public interface IWindowService
{
    void Show<TView>(object viewModel) where TView : Window, new();
    bool? ShowDialog<TView>(object viewModel) where TView : Window, new();
    void Close();
}