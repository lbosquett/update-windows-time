using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace UpdateWindowsTime.Time;

/// <summary>
/// Represents a client which can obtain accurate time via NTP protocol.
/// https://stackoverflow.com/a/12150289/1724128
/// CC BY-SA 3.0
/// </summary>
public class NtpClient : ITimeSource
{
    private readonly string _ntpServer;

    public NtpClient(string ntpServer)
    {
        _ntpServer = ntpServer;
    }
    
    /*
     * Retrieve the network time using a NTP server
     */
    public DateTime GetTime()
    {
        const int daysTo1900 = 1900 * 365 + 95; // 95 = offset for leap-years etc.
        const long ticksPerSecond = 10000000L;
        const long ticksPerDay = 24 * 60 * 60 * ticksPerSecond;
        const long ticksTo1900 = daysTo1900 * ticksPerDay;

        var ntpData = new byte[48];
        ntpData[0] = 0x1B; // LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

        var addresses = Dns.GetHostEntry(_ntpServer).AddressList;
        var ipEndPoint = new IPEndPoint(addresses[0], 123);
        // ReSharper disable once RedundantAssignment
        var pingDuration = Stopwatch.GetTimestamp(); // temp access (JIT-Compiler need some time at first call)
        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Connect(ipEndPoint);
            socket.ReceiveTimeout = 5000;
            socket.Send(ntpData);
            pingDuration = Stopwatch.GetTimestamp(); // after Send-Method to reduce WinSocket API-Call time

            socket.Receive(ntpData);
            pingDuration = Stopwatch.GetTimestamp() - pingDuration;
        }

        var pingTicks = pingDuration * ticksPerSecond / Stopwatch.Frequency;

        var intPart = (long) ntpData[40] << 24 | (long) ntpData[41] << 16 | (long) ntpData[42] << 8 | ntpData[43];
        var fractalPart = (long) ntpData[44] << 24 | (long) ntpData[45] << 16 | (long) ntpData[46] << 8 | ntpData[47];
        var netTicks = intPart * ticksPerSecond + (fractalPart * ticksPerSecond >> 32);

        var networkDateTime = new DateTime(ticksTo1900 + netTicks + pingTicks / 2, DateTimeKind.Utc);

        return networkDateTime.ToUniversalTime();
    }
}