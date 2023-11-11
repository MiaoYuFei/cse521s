using firmware.Command;
using firmware.Response;
using System.Collections.Generic;
using System.IO.Ports;
using static firmware.Command.SetRegionCommand;

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

        public RFIDReaderConnectionClosedEventHandler ConnectionClosedEventHandler { get; private set; }

        private volatile bool m_IsOpen = false;

        public bool IsOpen { get => m_IsOpen; }

        public string Hardware { get; private set; }

        public string Software { get; private set; }

        public string Manufacturer { get; private set; }

        public ushort TXPower { get; private set; }

        public RegionCodeEnum RegionCode { get; private set; }

        public bool FrequencyHopping { get; private set; }

        private readonly ReaderWriterLockSlim m_TagsLock = new();

        private readonly Dictionary<string, DateTime> m_TagsActivity = new();

        private DateTime m_TagLastTime = DateTime.UtcNow;

        public HashSet<string> Tags
        {
            get
            {
                m_TagsLock.EnterReadLock();
                try
                {
                    return new HashSet<string>(m_TagsActivity.Keys);
                }
                finally
                {
                    m_TagsLock.ExitReadLock();
                }
            }
        }

        private Thread ? m_ThreadTag;

        public RFIDReader(string portName, int baudRate = 115200)
        {
            m_IsOpen = false;
            m_PortName = portName;
            m_BaudRate = baudRate;
            m_SerialPort = new(m_PortName, m_BaudRate, Parity.None, 8, StopBits.One);
            Hardware = "N/A";
            Software = "N/A";
            Manufacturer = "N/A";
            TXPower = 0;
            RegionCode = RegionCodeEnum.US;
            FrequencyHopping = false;

            DataFrameReceivedEventHandler = new RFIDReaderDataFrameReceivedEventHandler(ProcessDataFrame);
            ConnectionClosedEventHandler = new RFIDReaderConnectionClosedEventHandler(ProcessConnectionClosed);

            m_Sender = new RFIDReaderSender(this);
            m_Receiver = new RFIDReaderReceiver();
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
                m_Receiver.RFIDReaderConnectionClosed += ConnectionClosedEventHandler;
                m_IsOpen = true;
                this.SendCommand(new StopMultipleInventoryCommand());
                this.SendCommand(new GetModuleInformationCommand(GetModuleInformationCommand.ModuleInfoTypeEnum.HARDWARE));
                this.SendCommand(new GetModuleInformationCommand(GetModuleInformationCommand.ModuleInfoTypeEnum.SOFTWARE));
                this.SendCommand(new GetModuleInformationCommand(GetModuleInformationCommand.ModuleInfoTypeEnum.MANUFACTURER));
                this.SendCommand(new SetRegionCommand(RegionCodeEnum.US));
                this.SendCommand(new GetRegionCommand());
                this.SendCommand(new SetTXPowerCommand(26));
                this.SendCommand(new GetTXPowerCommand());
                this.SendCommand(new FrequencyHoppingCommand(true));

                m_ThreadTag = new Thread(ProcGetTags);
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
            if (m_IsOpen)
            {
                this.SendCommand(new StopMultipleInventoryCommand());
                m_IsOpen = false;
            }
            m_ThreadTag?.Join();
            m_ThreadTag = null;
            m_Sender.Close();
            m_SerialPort.DataReceived -= m_Receiver.RFIDReaderRawDataReceivedEventHandler;
            m_Receiver.RFIDReaderDataFrameReceived -= DataFrameReceivedEventHandler;
            m_Receiver.RFIDReaderConnectionClosed -= ConnectionClosedEventHandler;
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
            try
            {
                baseResponse = new(e.DataFrame);
            }
            catch (Exception)
            {
                return;
            }
            switch (baseResponse.CommandType)
            {
                case (CommandTypeEnum.EXE_FAILED):
                    {
                        if (baseResponse.Parameter[0] == 0x15)
                        {
                            m_TagLastTime = DateTime.UtcNow;
                        }
                        break;
                    }
                case (CommandTypeEnum.GET_MODULE_INFO):
                    {
                        GetModuleInformationAnswerResponse response = new(baseResponse);
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
                        TXPower = response.Power;
                        break;
                    }
                case (CommandTypeEnum.GET_REGION):
                    {
                        GetRegionAnswerResponse response = new(baseResponse);
                        RegionCode = response.RegionCode;
                        break;
                    }
                case (CommandTypeEnum.SET_FHSS):
                    {
                        FrequencyHopping = true;
                        break;
                    }
                case (CommandTypeEnum.INVENTORY):
                    {
                        m_TagLastTime = DateTime.UtcNow;
                        InventoryNoticeResponse response = new(baseResponse);
                        m_TagsLock.EnterWriteLock();
                        try
                        {
                            string epc = Util.GetHexStringFromBytes(response.EPC);
                            m_TagsActivity[epc] = DateTime.UtcNow;
                        }
                        finally
                        {
                            m_TagsLock.ExitWriteLock();
                        }
                        break;
                    }
            }
        }

        private void ProcessConnectionClosed(object sender, RFIDReaderConnectionClosedEventArgs e) {
            Close();
        }

        private void ProcGetTags()
        {
            this.SendCommand(new MultipleInventoryCommand(10000));
            while (m_IsOpen)
            {
                m_TagsLock.EnterWriteLock();
                try
                {
                    IEnumerable<string> keysToRemove = m_TagsActivity.Where(pair => (DateTime.UtcNow - pair.Value).TotalMilliseconds > 1000).Select(pair => pair.Key);
                    foreach (var key in keysToRemove)
                    {
                        m_TagsActivity.Remove(key);
                    }
                }
                finally
                {
                    m_TagsLock.ExitWriteLock();
                }
                TimeSpan timeDifference = DateTime.UtcNow - m_TagLastTime;
                if (timeDifference.TotalMilliseconds >= 1000)
                {
                    this.SendCommand(new StopMultipleInventoryCommand());
                    this.SendCommand(new MultipleInventoryCommand(10000));
                }
                Thread.Sleep(500);
            }
        }
    }
}
