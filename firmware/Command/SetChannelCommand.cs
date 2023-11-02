namespace firmware.Command
{
    public class SetChannelCommand : BaseCommand
    {
        private byte m_ChannelIndex = 0;

        public byte ChannelIndex
        {
            get => m_ChannelIndex;
            private set
            {
                m_ChannelIndex = value;
                Parameter[0] = ChannelIndex;
            }
        }

        public SetChannelCommand(byte channelIndex) : base(1)
        {
            CommandType = CommandTypeEnum.SET_RF_CHANNEL;

            ChannelIndex = channelIndex;
        }
    }
}
