using System.Windows;

namespace WpfApp1;

public class WindowCloser
{
    public static readonly DependencyProperty EnableWindowClosingProperty = DependencyProperty.RegisterAttached(
        "EnableWindowClosing", typeof(bool), typeof(WindowCloser), new PropertyMetadata(default(bool)
            , OnEnableWindowChanged));


    public static void SetEnableWindowClosing(DependencyObject element, bool value)
    {
        element.SetValue(EnableWindowClosingProperty, value);
    }

    public static bool GetEnableWindowClosing(DependencyObject element)
    {
        return (bool)element.GetValue(EnableWindowClosingProperty);
    }

    private static void OnEnableWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
        {
            window.Loaded += (sender, args) =>
                {
                    if ((bool)e.NewValue)
                    {
                        window.Closing += (sender, args) =>
                            {
                                if (window.DataContext is ICloseWindows viewModel)
                                {
                                    viewModel.Close();
                                }

                                window.Closing += (sender, args) =>
                                    {
                                        if (window.DataContext is ICloseWindows viewModel)
                                        {
                                            args.Cancel = !viewModel.CanClose();
                                        }
                                    };
                            };
                    }
                };
        }
    }

}