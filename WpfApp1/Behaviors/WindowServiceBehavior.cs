using System.Windows;
using System.Windows.Interactivity;

namespace WpfApp1.Behaviors;

public class WindowServiceBehavior : Behavior<FrameworkElement>
{
    private Window dialogWindow = null!;
    
    public static readonly DependencyProperty ViewTemplateProperty = DependencyProperty.Register(
        nameof(ViewTemplate),
        typeof(DataTemplate),
        typeof(WindowServiceBehavior),
        new PropertyMetadata(null));

    public DataTemplate ViewTemplate
    {
        get => (DataTemplate)GetValue(ViewTemplateProperty);
        set => SetValue(ViewTemplateProperty, value);
    }

    public static readonly DependencyProperty WindowStyleProperty = DependencyProperty.Register(
        nameof(WindowStyle),
        typeof(Style),
        typeof(WindowServiceBehavior),
        new PropertyMetadata(null));

    public Style WindowStyle
    {
        get => (Style)GetValue(WindowStyleProperty);
        set => SetValue(WindowStyleProperty, value);
    }

    public void Show(object viewModel)
    {
        if (ViewTemplate == null)
            throw new InvalidOperationException("ViewTemplate is required to show the window.");

        // Create the dialog window
        dialogWindow = new Window
            {
                Content = ViewTemplate.LoadContent(),
                DataContext = viewModel,
                Style = WindowStyle,
                Owner = Application.Current.MainWindow
            };
        
        dialogWindow.Show();
    }

    public bool? ShowDialog(object viewModel)
    {
        if (ViewTemplate == null)
            throw new InvalidOperationException("ViewTemplate is required to show the dialog.");

        // Create the dialog window
        dialogWindow = new Window
            {
                Content = ViewTemplate.LoadContent(),
                DataContext = viewModel,
                Style = WindowStyle,
                Owner = Application.Current.MainWindow
            };
        
        return dialogWindow.ShowDialog();
    }

    public void Close()
    {
        dialogWindow?.Close();
        dialogWindow = null!;
    }
}