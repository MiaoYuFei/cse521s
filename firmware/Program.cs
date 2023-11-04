using firmware;
using firmware.Command;
using firmware.Response;
using System;
using System.Collections;
using System.IO.Ports;
using System.Runtime.InteropServices;

class Program
{

    static RFIDReader? rfidReader;

    static void Main(string[] args)
    {
        AppDomain.CurrentDomain.ProcessExit += ProcApplicationExit;
        string ? portName = null;
        string[] ? portNames = null;
        while (portNames == null || portNames.Length <= 0) {
            portNames = SerialPort.GetPortNames();
            Thread.Sleep(5000);
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

        rfidReader = new(portName);
        rfidReader.Open();
        Thread threadTags = new(() =>
        {
            while (true)
            {
                Thread.Sleep(500);
                Console.WriteLine("-->>--");
                HashSet<string> tags = rfidReader.Tags;
                foreach (string tag in tags)
                {
                    Console.WriteLine(tag);
                }
                Console.WriteLine("--<<--");
            }
        });
        threadTags.Start();
        Thread.Sleep(60000);
        threadTags.Join();
        rfidReader.Close();
    }

    private static void ProcApplicationExit(object? sender, EventArgs e)
    {
        rfidReader?.Close();
    }

}
