using System.Windows;
using System.Windows.Input;
using Minimoniclock.ViewModels;

namespace Minimoniclock;

public partial class MainWindow : Window
{
    private WindowState _previousWindowState;
    private WindowStyle _previousWindowStyle;
    private ResizeMode _previousResizeMode;
    private bool _fitModeEnabled;

    public MainWindow(LaunchOptions? launchOptions = null)
    {
        InitializeComponent();
        ApplyLaunchOptions(launchOptions);
        Loaded += OnLoaded;
        Closing += (_, _) => (DataContext as IDisposable)?.Dispose();
    }

    private void ApplyLaunchOptions(LaunchOptions? launchOptions)
    {
        if (launchOptions?.Width is not null && launchOptions.Height is not null)
        {
            Width = launchOptions.Width.Value;
            Height = launchOptions.Height.Value;
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ClockViewModel viewModel)
        {
            viewModel.UpdateLayout(ActualWidth, ActualHeight);
        }
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (DataContext is ClockViewModel viewModel)
        {
            viewModel.UpdateLayout(e.NewSize.Width, e.NewSize.Height);
        }
    }

    private void OnTopmostChanged(object sender, RoutedEventArgs e)
    {
        Topmost = TopmostToggle.IsChecked == true;
    }

    private void OnFitModeClicked(object sender, RoutedEventArgs e)
    {
        ToggleFitMode();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F11)
        {
            ToggleFitMode();
            e.Handled = true;
        }
        else if (e.Key == Key.Escape && _fitModeEnabled)
        {
            ToggleFitMode();
            e.Handled = true;
        }
        else if (e.Key == Key.T)
        {
            TopmostToggle.IsChecked = !TopmostToggle.IsChecked;
            e.Handled = true;
        }
    }

    private void ToggleFitMode()
    {
        if (_fitModeEnabled)
        {
            WindowStyle = _previousWindowStyle;
            ResizeMode = _previousResizeMode;
            WindowState = _previousWindowState;
            _fitModeEnabled = false;
            return;
        }

        _previousWindowState = WindowState;
        _previousWindowStyle = WindowStyle;
        _previousResizeMode = ResizeMode;

        WindowStyle = WindowStyle.None;
        ResizeMode = ResizeMode.NoResize;
        WindowState = WindowState.Maximized;
        _fitModeEnabled = true;
    }
}
