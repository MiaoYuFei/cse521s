using firmware;
using firmware.Command;
using firmware.Response;
using System;
using System.Collections;
using System.IO.Ports;

class Program
{
    static void Main(string[] args)
    {
        string[] portNames = SerialPort.GetPortNames();
        RFIDReader reader = new(portNames[0]);
        reader.Open();
        Thread th = new(() =>
        {
            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("----");
                foreach (string tag in reader.Tags)
                {
                    Console.WriteLine(tag);
                }
                Console.WriteLine("----");
            }
        });
        th.Start();
        Thread.Sleep(20000);
        th.Join();
        reader.Close();
    }

}
