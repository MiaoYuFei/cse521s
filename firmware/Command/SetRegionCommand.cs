namespace firmware.Command
{
    public class SetRegionCommand : BaseCommand
    {
        public enum RegionCodeEnum : byte
        {
            CHN2 = 0x01,
            US = 0x02,
            EUR = 0x03,
            CHN1 = 0x04,
            JAPAN = 0x05,
            KOREA = 0x06
        }

        private RegionCodeEnum m_RegionCode = 0;

        public RegionCodeEnum RegionCode
        {
            get => m_RegionCode;
            private set
            {
                m_RegionCode = value;
                Parameter[0] = (byte)m_RegionCode;
            }
        }

        public SetRegionCommand(RegionCodeEnum regionCode) : base(1)
        {
            CommandType = CommandTypeEnum.SET_REGION;

            RegionCode = regionCode;
        }
    }
}
