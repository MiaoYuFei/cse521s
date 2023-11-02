using firmware.Command;
using firmware.Response;
using System.IO.Ports;

namespace firmware
{
    public class RFIDReader
    {
        private SerialPort m_SerialPort;
        private readonly string m_PortName;
        private readonly int m_BaudRate;

        private readonly RFIDReaderSender m_Sender;
        private readonly RFIDReaderReceiver m_Receiver;

        public RFIDReaderDataFrameReceivedEventHandler DataFrameReceivedEventHandler { get; private set; }
        
        private volatile bool m_IsOpen = false;

        public bool IsOpen { get => m_IsOpen; }

        public string Hardware { get; private set; }

        public string Software { get; private set; }

        public string Manufacturer { get; private set; }

        private readonly ReaderWriterLockSlim m_TagsLock = new();

        public HashSet<string> m_Tags = new();

        public HashSet<string> m_TagsNew = new();

        public HashSet<string> Tags
        {
            get
            {
                m_TagsLock.EnterReadLock();
                try
                {
                    return new HashSet<string>(m_Tags);
                }
                finally
                {
                    m_TagsLock.ExitReadLock();
                }
            }
        }

        private readonly Thread m_ThreadTag;

        public RFIDReader(string portName, int baudRate = 115200)
        {
            m_IsOpen = false;
            m_PortName = portName;
            m_BaudRate = baudRate;
            m_SerialPort = new(m_PortName, m_BaudRate, Parity.None, 8, StopBits.One);
            Hardware = "N/A";
            Software = "N/A";
            Manufacturer = "N/A";

            DataFrameReceivedEventHandler = new RFIDReaderDataFrameReceivedEventHandler(ProcessDataFrame);

            m_Sender = new RFIDReaderSender(this);
            m_Receiver = new RFIDReaderReceiver();

            m_ThreadTag = new Thread(ProcGetTags);
        }

        public bool Open()
        {
            if (m_SerialPort.IsOpen)
            {
                return true;
            }
            try
            {
                m_SerialPort.Open();
                m_Sender.Open();
                m_SerialPort.DataReceived += m_Receiver.RFIDReaderRawDataReceivedEventHandler;
                m_Receiver.RFIDReaderDataFrameReceived += DataFrameReceivedEventHandler;
                m_IsOpen = true;
                this.SendCommand(new GetModuleInformationCommand(GetModuleInformationCommand.ModuleInfoTypeEnum.HARDWARE));
                this.SendCommand(new GetModuleInformationCommand(GetModuleInformationCommand.ModuleInfoTypeEnum.SOFTWARE));
                this.SendCommand(new GetModuleInformationCommand(GetModuleInformationCommand.ModuleInfoTypeEnum.MANUFACTURER));
                m_ThreadTag.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendCommand(BaseCommand command)
        {
            return m_Sender.Send(command);
        }

        internal bool SendBytes(byte[] data)
        {
            if (!m_SerialPort.IsOpen)
            {
                return false;
            }
            try
            {
                m_SerialPort.Write(data, 0, data.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Close()
        {
            m_IsOpen = false;
            m_ThreadTag.Join();
            m_Sender.Close();
            m_SerialPort.DataReceived -= m_Receiver.RFIDReaderRawDataReceivedEventHandler;
            m_Receiver.RFIDReaderDataFrameReceived -= DataFrameReceivedEventHandler;
            try
            {
                m_SerialPort.Close();
            }
            catch (Exception)
            {
                m_SerialPort.Dispose();
                m_SerialPort = new SerialPort(m_PortName, m_BaudRate, Parity.None, 8, StopBits.One);
            }
        }

        private void ProcessDataFrame(object sender, RFIDReaderDataFrameReceivedEventArgs e)
        {
            BaseResponse baseResponse;
            try {
                baseResponse = new(e.DataFrame);
            } catch (Exception) {
                return;
            }
            switch (baseResponse.CommandType)
            {
                case (CommandTypeEnum.EXE_FAILED):
                    {
                        break;
                    }
                case (CommandTypeEnum.GET_MODULE_INFO):
                    {
                        GetModuleInformationAnswerResponse response = new GetModuleInformationAnswerResponse(baseResponse);
                        switch (response.ModuleInfoType)
                        {
                            case GetModuleInformationCommand.ModuleInfoTypeEnum.HARDWARE:
                                {
                                    this.Hardware = response.Info;
                                    break;
                                }
                            case GetModuleInformationCommand.ModuleInfoTypeEnum.SOFTWARE:
                                {
                                    this.Software = response.Info;
                                    break;
                                }
                            case GetModuleInformationCommand.ModuleInfoTypeEnum.MANUFACTURER:
                                {
                                    this.Manufacturer = response.Info;
                                    break;
                                }
                        }
                        break;
                    }
                case (CommandTypeEnum.GET_POWER):
                    {
                        GetTXPowerAnswerResponse response = new(baseResponse);
                        Console.WriteLine(response.Power + "dBm");
                        break;
                    }
                case (CommandTypeEnum.INVENTORY):
                    {
                        InventoryNoticeResponse response = new(baseResponse);
                        m_TagsLock.EnterWriteLock();
                        try
                        {
                            m_TagsNew.Add(Util.GetHexStringFromBytes(response.EPC));
                        }
                        finally
                        {
                            m_TagsLock.ExitWriteLock();
                        }
                        break;
                    }
            }
        }

        private void ProcGetTags()
        {
            while (m_IsOpen)
            {
                this.SendCommand(new MultipleInventoryCommand(1000));
                Thread.Sleep(1000);
                this.SendCommand(new StopMultipleInventoryCommand());
                m_TagsLock.EnterWriteLock();
                try
                {
                    (m_TagsNew, m_Tags) = (m_Tags, m_TagsNew);
                }
                finally
                {
                    m_TagsLock.ExitWriteLock();
                }
                m_TagsNew.Clear();
            }
        }

    }
}
