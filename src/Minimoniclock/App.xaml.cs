using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace Minimoniclock;

public partial class App : Application
{
    private static readonly string LogDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "minimoniclock",
        "logs");

    private static readonly string LogPath = Path.Combine(
        LogDirectory,
        $"minimoniclock-{DateTime.Now:yyyyMMdd}.log");

    public App()
    {
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            new MainWindow().Show();
        }
        catch (Exception exception)
        {
            ReportFatalError("Startup failed.", exception);
            Shutdown(1);
        }
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        ReportFatalError("Unhandled UI error.", e.Exception);
        e.Handled = true;
        Shutdown(1);
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
        {
            WriteLog("Unhandled application error.", exception);
        }
        else
        {
            WriteLog($"Unhandled application error: {e.ExceptionObject}");
        }
    }

    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        WriteLog("Unobserved task error.", e.Exception);
        e.SetObserved();
    }

    private static void ReportFatalError(string message, Exception exception)
    {
        WriteLog(message, exception);
        MessageBox.Show(
            $"{message}{Environment.NewLine}{exception.Message}{Environment.NewLine}{Environment.NewLine}Log: {LogPath}",
            "minimoniclock",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }

    private static void WriteLog(string message, Exception exception)
    {
        WriteLog($"{message}{Environment.NewLine}{exception}");
    }

    private static void WriteLog(string message)
    {
        Directory.CreateDirectory(LogDirectory);
        File.AppendAllText(
            LogPath,
            $"[{DateTimeOffset.Now:O}] {message}{Environment.NewLine}{Environment.NewLine}");
    }
}
