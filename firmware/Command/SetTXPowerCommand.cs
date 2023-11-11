using System.Buffers.Binary;

namespace firmware.Command
{
    public class SetTXPowerCommand : BaseCommand
    {

        private ushort m_TXPower = 0;

        public ushort TXPower
        {
            get => m_TXPower;
            private set
            {
                m_TXPower = value;
                byte[] txPowerBytes = new byte[sizeof(ushort)];
                BinaryPrimitives.WriteUInt16BigEndian(txPowerBytes, (ushort)(m_TXPower * 100));
                Array.Copy(txPowerBytes, 0, Parameter, 0, sizeof(ushort));
            }
        }

        public SetTXPowerCommand(ushort power) : base(2)
        {
            CommandType = CommandTypeEnum.SET_POWER;

            TXPower = power;
        }
    }
}
