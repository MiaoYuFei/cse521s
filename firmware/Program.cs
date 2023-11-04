using firmware;
using firmware.Command;
using firmware.Response;
using System;
using System.Collections;
using System.IO.Ports;
using System.Runtime.InteropServices;

class Program
{
    static void Main(string[] args)
    {
        string? portName = null;
        string[] portNames = SerialPort.GetPortNames();
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

        RFIDReader reader = new(portName);
        reader.Open();
        Thread th = new(() =>
        {
            while (true)
            {
                Thread.Sleep(500);
                Console.WriteLine("-->>--");
                HashSet<string> tags = reader.Tags;
                foreach (string tag in tags)
                {
                    Console.WriteLine(tag);
                }
                Console.WriteLine("--<<--");
            }
        });
        th.Start();
        Thread.Sleep(20000);
        th.Join();
        reader.Close();
    }

}
