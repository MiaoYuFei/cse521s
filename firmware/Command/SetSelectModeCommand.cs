namespace firmware.Command
{
    public class SetSelectModeCommand : BaseCommand
    {
        public enum SelectModeEnum : byte
        {
            SEND_BEFORE = 0x00,
            NO_SEND_BEFORE = 0x01,
            SEND_BEFORE_EXCEPT_INVENTORY = 0x02
        }

        private SelectModeEnum m_SelectMode = SelectModeEnum.SEND_BEFORE;

        public SelectModeEnum SelectMode
        {
            get => m_SelectMode;
            private set
            {
                m_SelectMode = value;
                Parameter[0] = (byte)m_SelectMode;
            }
        }

        public SetSelectModeCommand(SelectModeEnum selectMode) : base(1)
        {
            CommandType = CommandTypeEnum.SET_INVENTORY_MODE;

            SelectMode = selectMode;
        }
    }
}
