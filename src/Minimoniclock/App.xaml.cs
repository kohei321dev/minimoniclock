using System.Globalization;
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
            var launchOptions = LaunchOptions.Parse(e.Args);
            new MainWindow(launchOptions).Show();
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

public sealed record LaunchOptions(double? Width, double? Height)
{
    public static LaunchOptions Parse(string[] args)
    {
        double? width = null;
        double? height = null;

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (arg.Equals("--size", StringComparison.OrdinalIgnoreCase)
                || arg.Equals("--window-size", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 >= args.Length)
                {
                    throw new ArgumentException($"{arg} requires a value like 1280x400.");
                }

                (width, height) = ParseSize(args[++i]);
            }
            else if (arg.StartsWith("--size=", StringComparison.OrdinalIgnoreCase))
            {
                (width, height) = ParseSize(arg["--size=".Length..]);
            }
            else if (arg.StartsWith("--window-size=", StringComparison.OrdinalIgnoreCase))
            {
                (width, height) = ParseSize(arg["--window-size=".Length..]);
            }
        }

        return new LaunchOptions(width, height);
    }

    private static (double Width, double Height) ParseSize(string value)
    {
        var parts = value.Split('x', 'X');
        if (parts.Length != 2
            || !double.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out var width)
            || !double.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var height)
            || width <= 0
            || height <= 0)
        {
            throw new ArgumentException($"Invalid window size: {value}. Use a value like 1280x400.");
        }

        return (width, height);
    }
}
