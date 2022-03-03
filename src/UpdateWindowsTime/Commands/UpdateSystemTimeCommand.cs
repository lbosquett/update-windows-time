using UpdateWindowsTime.Console;
using UpdateWindowsTime.Time;

namespace UpdateWindowsTime.Commands;

public class UpdateSystemTimeCommand : ICommand
{
    private readonly UpdateSystemTimeArguments _arguments;
    private readonly ITimeSource _timeSource;

    public UpdateSystemTimeCommand(ITimeSource timeSource, UpdateSystemTimeArguments arguments)
    {
        _timeSource = timeSource;
        _arguments = arguments;
    }
    
    public void Execute()
    {
        var dateTime = _timeSource.GetTime();
        
        var updatedTime = new WindowsSystemTime.SystemTime
        {
            Year = (ushort)dateTime.Year,
            Month = (ushort)dateTime.Month,
            Day = (ushort)dateTime.Day,
            Hour = (ushort)dateTime.Hour,
            Minute = (ushort)dateTime.Minute,
            Second = (ushort)dateTime.Second
        };

        var updated = WindowsSystemTime.Win32SetSystemTime(ref updatedTime);
        if (!updated)
        {
            throw new Exception("cannot update system time");
        }
    }
}