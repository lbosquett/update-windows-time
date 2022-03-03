using UpdateWindowsTime.Console;
using Xunit;

namespace UpdateWindowsTime.Test;

public class UnitTest1
{
    [Fact]
    public void ReadArgument()
    {
        IArgumentReader<UpdateSystemTimeArguments> reader = new UpdateSystemTimeArgumentReader();
        reader.Read("--url");
        reader.Read("example.org");

        var result = reader.GetResult();
        
        Assert.Equal("example.org", result.Url);
    }

    [Fact]
    public void DefaultUrl()
    {
        IArgumentReader<UpdateSystemTimeArguments> reader = new UpdateSystemTimeArgumentReader();
        var result = reader.GetResult();
        
        Assert.Equal(UpdateSystemTimeArgumentReader.DefaultNtpServer, result.Url);
    }
}