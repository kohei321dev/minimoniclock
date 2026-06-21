using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace Minimoniclock.ViewModels;

public sealed class ClockViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly DispatcherTimer _timer;
    private string _timeText = ClockFormatter.Format(DateTime.Now);
    private string _layoutLabel = "PORTRAIT";
    private Thickness _clockMargin = new(24);

    public ClockViewModel()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += (_, _) => TimeText = ClockFormatter.Format(DateTime.Now);
        _timer.Start();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string TimeText
    {
        get => _timeText;
        private set => SetField(ref _timeText, value);
    }

    public string LayoutLabel
    {
        get => _layoutLabel;
        private set => SetField(ref _layoutLabel, value);
    }

    public Thickness ClockMargin
    {
        get => _clockMargin;
        private set => SetField(ref _clockMargin, value);
    }

    public void UpdateLayout(double width, double height)
    {
        if (width <= 0 || height <= 0)
        {
            return;
        }

        var isPortrait = height >= width;
        LayoutLabel = isPortrait ? "PORTRAIT" : "LANDSCAPE";
        ClockMargin = isPortrait
            ? new Thickness(18, 80, 18, 80)
            : new Thickness(56, 20, 56, 20);
    }

    public void Dispose()
    {
        _timer.Stop();
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
