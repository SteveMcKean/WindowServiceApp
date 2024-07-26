using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Views;

public partial class Notification : UserControl
{
    public Notification()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var window = this.Parent as Window;
        window.DialogResult = true;
        window.Close();
        
    }
}