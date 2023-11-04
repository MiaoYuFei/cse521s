using System.IO.Ports;

namespace firmware
{
    public class RFIDReaderDataFrameReceivedEventArgs : EventArgs
    {
        public byte[] DataFrame { get; private set; }
        public RFIDReaderDataFrameReceivedEventArgs(byte[] dataFrame)
        {
            DataFrame = dataFrame;
        }
    }

    public delegate void RFIDReaderDataFrameReceivedEventHandler(object sender, RFIDReaderDataFrameReceivedEventArgs e);

    public class RFIDReaderConnectionClosedReceivedEventArgs : EventArgs
    { }

    public delegate void RFIDReaderConnectionClosedEventHandler(object sender, RFIDReaderConnectionClosedReceivedEventArgs e);

    internal class RFIDReaderReceiver
    {
        public SerialDataReceivedEventHandler RFIDReaderRawDataReceivedEventHandler { get; private set;}

        public event RFIDReaderDataFrameReceivedEventHandler? RFIDReaderDataFrameReceived;

        public event RFIDReaderConnectionClosedEventHandler? RFIDReaderConnectionClosed;

        private enum StateEnum
        {
            Idle,
            InFrame,
        }

        private readonly List<byte> m_Buffer = new();
        private StateEnum m_State = StateEnum.Idle;
        private ushort m_ParameterLength = 0;

        public RFIDReaderReceiver()
        {
            RFIDReaderRawDataReceivedEventHandler = new SerialDataReceivedEventHandler(ProcRawData);
        }

        public void ProcRawData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort reader = (SerialPort)sender;
            if (!reader.IsOpen)
            {
                RFIDReaderConnectionClosed?.Invoke(sender, new RFIDReaderConnectionClosedReceivedEventArgs());
                return;
            }
            int bytesToRead = reader.BytesToRead;
            if (bytesToRead == 0)
            {
                RFIDReaderConnectionClosed?.Invoke(sender, new RFIDReaderConnectionClosedReceivedEventArgs());
                return;
            }

            byte[] receivedData = new byte[bytesToRead];
            reader.Read(receivedData, 0, bytesToRead);

            foreach (byte b in receivedData)
            {
                switch (m_State)
                {
                    case StateEnum.Idle:
                        {
                            if (b == (byte)ConstantEnum.HeaderFlag)
                            {
                                m_Buffer.Clear();
                                m_Buffer.Add(b);
                                m_State = StateEnum.InFrame;
                            }
                            break;
                        }
                    case StateEnum.InFrame:
                        {
                            m_Buffer.Add(b);
                            if (m_Buffer.Count == 5)
                            {
                                m_ParameterLength = (ushort)(m_Buffer[3] << 8 | b);
                            }
                            else if (m_Buffer.Count >= (m_ParameterLength + 7) && b == (byte)ConstantEnum.EndingFlag)
                            {
                                m_ParameterLength = 0;
                                m_State = StateEnum.Idle;
                                RFIDReaderDataFrameReceived?.Invoke(sender, new RFIDReaderDataFrameReceivedEventArgs(m_Buffer.ToArray()));
                            }
                            break;
                        }
                }
            }
        }
    }
}
