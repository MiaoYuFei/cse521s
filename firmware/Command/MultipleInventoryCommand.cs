using System.Buffers.Binary;

namespace firmware.Command
{
    public class MultipleInventoryCommand : BaseCommand
    {

        private ushort m_Count = 0;

        public ushort Count
        {
            get => m_Count;
            private set
            {
                m_Count = value;
                Span<byte> countBytes = new byte[sizeof(ushort)];
                BinaryPrimitives.WriteUInt16BigEndian(countBytes, m_Count);
                Array.Copy(countBytes.ToArray(), 0, Parameter, 1, sizeof(ushort));
            }
        }

        public MultipleInventoryCommand(ushort count) : base(3)
        {
            CommandType = CommandTypeEnum.READ_MULTI;

            Count = count;
            Parameter[0] = 0x22;
        }
    }
}
