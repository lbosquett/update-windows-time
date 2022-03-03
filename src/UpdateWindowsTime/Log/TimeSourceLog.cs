using UpdateWindowsTime.Time;

namespace UpdateWindowsTime.Log;

public class TimeSourceLog : ITimeSource
{
    private readonly ITimeSource _source;

    public TimeSourceLog(ITimeSource source)
    {
        _source = source;
    }
    
    public DateTime GetTime()
    {
        DateTime result = _source.GetTime();
        System.Console.WriteLine($"Retrieved {result.ToLocalTime()}");
        return result;
    }
}