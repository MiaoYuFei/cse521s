namespace firmware.Command
{
    public class LockCommand : BaseCommand
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

        private byte[] m_LockPayload = Array.Empty<byte>();

        public byte[] LockPayload
        {
            get => m_LockPayload;
            private set
            {
                m_LockPayload = value;
                Array.Copy(m_LockPayload, 0, Parameter, 4, 3);
            }
        }

        public LockCommand(byte[] accessPassword, byte[] lockPayload) : base (7)
        {
            if (accessPassword.Length != 4)
            {
                throw new ArgumentException();
            }
            if (lockPayload.Length != 3)
            {
                throw new ArgumentException();
            }

            CommandType = CommandTypeEnum.LOCK_UNLOCK;

            AccessPassword = accessPassword;
            LockPayload = lockPayload;
        }
    }
}
