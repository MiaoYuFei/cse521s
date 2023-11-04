namespace firmware.Command
{
    public class KillCommand : BaseCommand
    {
        private byte[] m_KillPassword = Array.Empty<byte>();

        public byte[] KillPassword
        {
            get => m_KillPassword;
            private set
            {
                m_KillPassword = value;
                Array.Copy(m_KillPassword, 0, Parameter, 0, 4);
            }
        }

        public KillCommand(byte[] killPassword) : base(4)
        {
            if (killPassword.Length != 4)
            {
                throw new ArgumentException();
            }

            CommandType = CommandTypeEnum.KILL;

            KillPassword = killPassword;
        }
    }
}
