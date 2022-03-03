using System.Runtime.InteropServices;

namespace UpdateWindowsTime.Time;

public static class WindowsSystemTime
{
    public struct SystemTime
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Millisecond;
    };

    [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
    public static extern bool Win32SetSystemTime(ref SystemTime sysTime);
}