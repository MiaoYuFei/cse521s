using System.Buffers.Binary;

namespace firmware.Command
{
    public class WriteCommand : BaseCommand
    {
        private byte[] m_AccessPassword = Array.Empty<byte>();

        public byte[] AccessPassword
        {
            get => m_AccessPassword;
            private set
            {
                m_AccessPassword = value;
                Array.Copy(m_AccessPassword, 0, Parameter, 0, 4);
            }
        }

        private byte m_MemBank = 0x0;

        public byte MemBank
        {
            get => m_MemBank;
            private set
            {
                m_MemBank = value;
                Parameter[4] = m_MemBank;
            }
        }

        private byte[] m_SegmentAddress = Array.Empty<byte>();

        public byte[] SegmentAddress
        {
            get => m_SegmentAddress;
            private set
            {
                m_SegmentAddress = value;
                Array.Copy(m_SegmentAddress, 0, Parameter, 5, 2);
            }
        }

        private ushort m_DataLength = 0;

        public ushort DataLength
        {
            get => m_DataLength;
            private set
            {
                m_DataLength = value;
                Span<byte> dataLengthBytes = new byte[sizeof(int)];
                BinaryPrimitives.WriteUInt16BigEndian(dataLengthBytes, m_DataLength);
                Array.Copy(dataLengthBytes.ToArray(), 0, Parameter, 7, sizeof(ushort));
            }
        }

        private byte[] m_Data = Array.Empty<byte>();

        public byte[] Data
        {
            get => m_Data;
            private set
            {
                m_Data = value;
                Array.Copy(m_Data, 0, Parameter, 9, DataLength);
            }
        }

        public WriteCommand(byte[] accessPassword, byte memBank, byte[] segmentAddress, ushort dataLength, byte[] data) : base((ushort)(9 + dataLength))
        {
            if (accessPassword.Length != 4)
            {
                throw new ArgumentException();
            }
            if (segmentAddress.Length != 2)
            {
                throw new ArgumentException();
            }
            if (data.Length != dataLength)
            {
                throw new ArgumentException();
            }

            CommandType = CommandTypeEnum.WRITE_DATA;

            AccessPassword = accessPassword;
            MemBank = memBank;
            SegmentAddress = segmentAddress;
            DataLength = dataLength;
            Data = data;
        }
    }
}
