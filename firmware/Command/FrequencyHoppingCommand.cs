namespace firmware.Command
{
    public class FrequencyHoppingCommand : BaseCommand
    {

        private bool m_FrequencyHopping = true;

        public bool FrequencyHopping
        {
            get => m_FrequencyHopping;
            private set
            {
                m_FrequencyHopping = value;
                Parameter[0] = m_FrequencyHopping ? (byte)0xff : (byte)0x00;
            }
        }

        public FrequencyHoppingCommand(bool enable) : base(1)
        {
            CommandType = CommandTypeEnum.SET_FHSS;

            FrequencyHopping = enable;
        }
    }
}
