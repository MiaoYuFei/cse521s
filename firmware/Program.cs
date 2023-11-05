using Amazon.IotData;
using Amazon.IotData.Model;
using Amazon.Runtime;
using firmware;
using System.IO.Ports;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

class Program
{

    static RFIDReader? rfidReader;

    static readonly AmazonIotDataClient iotClient = new("https://a38tjqu822q0m8-ats.iot.us-east-2.amazonaws.com", new BasicAWSCredentials("AKIA4VOJVMETAAK4YAAV", "hlsGKxO8DL1V1o8FXd6YZztY6nC6HpSUFL/f1ldB"));

    static readonly string topicStatus = "RFIDReader/Status";

    static readonly string topicTagScanResult = "RFIDReader/TagScanResult";

    static void Main(string[] args)
    {
        AppDomain.CurrentDomain.ProcessExit += ProcApplicationExit;
        string? portName = null;
        string[]? portNames = null;
        while (portNames == null || portNames.Length <= 0)
        {
            portNames = SerialPort.GetPortNames();
            Thread.Sleep(3000);
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            portName = portNames[0];
        }
        else
        {
            foreach (string element in portNames)
            {
                if (element.Contains("/dev/ttyACM"))
                {
                    portName = element;
                }
            }
            if (portName == null)
            {
                throw new NotSupportedException();
            }
        }
        List<string> ipAddresses = new();
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var networkInterface in networkInterfaces)
        {
            if (networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
                foreach (var ipAddressInfo in ipProperties.UnicastAddresses)
                {
                    ipAddresses.Add(ipAddressInfo.Address.ToString());
                }
            }
        }

        Dictionary<string, object> responseStatus = new()
        {
            { "status", "start" },
            { "ip_addresses", ipAddresses }
        };
        var requestStatus = new PublishRequest
        {
            Topic = topicStatus,
            Payload = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responseStatus)))
        };
        iotClient.PublishAsync(requestStatus, CancellationToken.None).Wait();
        responseStatus.Clear();

        rfidReader = new(portName);
        if (!rfidReader.Open())
        {
            responseStatus["status"] = "connect_fail";
            requestStatus.Payload = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responseStatus)));
            iotClient.PublishAsync(requestStatus, CancellationToken.None).Wait();
            return;
        }

        responseStatus["status"] = "connect";
        requestStatus.Payload = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responseStatus)));
        iotClient.PublishAsync(requestStatus, CancellationToken.None).Wait();

        Thread threadTags = new(() =>
        {
            while (true)
            {
                Thread.Sleep(500);
                Dictionary<string, object> responseTagScanResult = new()
                {
                    { "tags", rfidReader.Tags }
                };
                var requestTagScanResult = new PublishRequest
                {
                    Topic = topicTagScanResult,
                    Payload = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responseTagScanResult)))
                };
                iotClient.PublishAsync(requestTagScanResult, CancellationToken.None).Wait();
            }
        });
        threadTags.Start();
        while (true)
        {
            Thread.Sleep(1000);
        }

    }

    private static void ProcApplicationExit(object? sender, EventArgs e)
    {
        rfidReader?.Close();
        Dictionary<string, object> responseStatus = new()
        {
            { "status", "stop" }
        };
        var requestStatus = new PublishRequest
        {
            Topic = topicStatus,
            Payload = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responseStatus)))
        };
        iotClient.PublishAsync(requestStatus, CancellationToken.None).Wait();
    }

}
