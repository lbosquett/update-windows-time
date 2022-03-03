using UpdateWindowsTime.Commands;
using UpdateWindowsTime.Console;
using UpdateWindowsTime.Log;
using UpdateWindowsTime.Time;

namespace UpdateWindowsTime;

internal static class Program
{
    internal static void Main(string[] args)
    {
        IArgumentReader<UpdateSystemTimeArguments> reader = new UpdateSystemTimeArgumentReader();
        foreach (var nextArgument in args)
        {
            reader.Read(nextArgument);
        }

        var arguments = reader.GetResult();
        System.Console.WriteLine($"getting time from {arguments.Url}");
        
        ITimeSource ntpSource = new NtpClient(arguments.Url);
        ITimeSource logSource = new TimeSourceLog(ntpSource);
        ICommand command = new UpdateSystemTimeCommand(logSource, reader.GetResult());
        command.Execute();

        System.Console.WriteLine("Updated Time");
    }
}