namespace UpdateWindowsTime.Console;

public class UpdateSystemTimeArgumentReader : IArgumentReader<UpdateSystemTimeArguments>
{
    public const string DefaultNtpServer = "time.nist.gov";
    
    private enum UpdateSystemTimeReadState
    {
        Initial = 0,
        Url
    }

    private UpdateSystemTimeReadState _state = UpdateSystemTimeReadState.Initial;

    private string? _url;

    private void ReadInitial(string argument)
    {
        switch (argument.ToLowerInvariant())
        {
            case "--url":
                _state = UpdateSystemTimeReadState.Url;
                break;
        }
    }

    private void ReadUrl(string urlArgument)
    {
        _url = urlArgument;
        _state = UpdateSystemTimeReadState.Initial;
    }

    public void Read(string argument)
    {
        switch (_state)
        {
            case UpdateSystemTimeReadState.Initial:
                ReadInitial(argument);
                break;
            case UpdateSystemTimeReadState.Url:
                ReadUrl(argument);
                break;
            default:
                throw new Exception($"invalid read state {_state}");
        }
    }

    public UpdateSystemTimeArguments GetResult()
    {
        return new UpdateSystemTimeArguments(_url ?? DefaultNtpServer);
    }
}