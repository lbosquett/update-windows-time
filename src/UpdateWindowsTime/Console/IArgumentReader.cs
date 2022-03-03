namespace UpdateWindowsTime.Console;

public interface IArgumentReader<out T>
{
    void Read(string argument);
    T GetResult();
}