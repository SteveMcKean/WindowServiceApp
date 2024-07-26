using System.Windows;

namespace WpfApp1.Attached;

public static class WindowCloser
{
    public static readonly DependencyProperty EnableWindowClosingProperty = DependencyProperty.RegisterAttached(
        "EnableWindowClosing", typeof(bool), typeof(WindowCloser), new PropertyMetadata(default(bool), PropertyChangedCallback));

    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
        {
            window.Loaded += (sender, args) =>
                {
                    if (window.DataContext is ICloseWindow closeable)
                    {
                        closeable.Close += () => window.Close();
                    }
                };
            
            window.Closing += (sender, args) =>
                {
                    if (window.DataContext is ICloseWindow closeable)
                    {
                        args.Cancel = !closeable.CanClose();
                    }
                    
                };
            
        }
        
    }

    public static void SetEnableWindowClosing(DependencyObject element, bool value)
    {
        element.SetValue(EnableWindowClosingProperty, value);
    }

    public static bool GetEnableWindowClosing(DependencyObject element)
    {
        return (bool)element.GetValue(EnableWindowClosingProperty);
    }
}