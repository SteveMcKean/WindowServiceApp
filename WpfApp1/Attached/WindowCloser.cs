using System.Windows;
using System.Windows.Controls;

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
                    if (window.DataContext is IWindowCloser closeable)
                    {
                        closeable.Close += () => window.Close();
                    }
                };
            
            window.Closing += (sender, args) =>
                {
                    if (window.DataContext is IWindowCloser closeable)
                    {
                        args.Cancel = !closeable.CanClose();
                    }
                    
                };
            
        }else if (d is UserControl userControl)
        {
            AttachUserControlCloser(userControl);
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

    private static void AttachUserControlCloser(UserControl userControl)
    {
        userControl.Loaded += (sender, args) =>
            {
                if (userControl.DataContext is IWindowCloser closeable)
                {
                    closeable.Close += () =>
                        {
                            // Find parent window and close it
                            if (FindParentWindow(userControl) is Window window)
                            {
                                window.Close();
                            }
                        };
                }
            };
    }

    private static Window FindParentWindow(DependencyObject child)
    {
        while (child != null)
        {
            if (child is Window window)
            {
                return window;
            }
            child = LogicalTreeHelper.GetParent(child);
        }
        return null;
    }
}