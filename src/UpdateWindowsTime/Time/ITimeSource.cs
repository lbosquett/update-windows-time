namespace UpdateWindowsTime.Time;

public interface ITimeSource
{
    DateTime GetTime();
}