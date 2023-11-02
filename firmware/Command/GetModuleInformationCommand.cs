using firmware.Response;

namespace firmware.Command
{
    public class GetModuleInformationCommand : BaseCommand
    {
        public enum ModuleInfoTypeEnum : byte
        {
            HARDWARE = 0x00,
            SOFTWARE = 0x01,
            MANUFACTURER = 0x02
        }

        private ModuleInfoTypeEnum m_ModuleInfoType = ModuleInfoTypeEnum.HARDWARE;

        public ModuleInfoTypeEnum ModuleInfoType {
            get => m_ModuleInfoType;
            private set
            {
                m_ModuleInfoType = value;
                Parameter[0] = (byte)m_ModuleInfoType;
            }
        }

        public GetModuleInformationCommand(ModuleInfoTypeEnum moduleInfoType) : base(1)
        {
            CommandType = CommandTypeEnum.GET_MODULE_INFO;

            ModuleInfoType = moduleInfoType;
        }
    }
}
