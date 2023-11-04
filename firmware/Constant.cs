namespace firmware
{
    public enum ConstantEnum
    {
        HeaderFlag = (byte)0xbb,
        EndingFlag = (byte)0x7e
    }

    public enum FrameTypeEnum : byte
    {
        COMMAND = 0x00,
        ANSWER = 0x01,
        NOTICE = 0x02
    }

    public enum CommandTypeEnum : byte
    {
        GET_MODULE_INFO = 0x03,
        SET_REGION = 0x07,
        GET_REGION = 0x08,
        SET_QUERY = 0x0e,
        GET_QUERY = 0x0d,
        SET_BAUDRATE = 0x11,
        INVENTORY = 0x22,
        READ_MULTI = 0x27,
        STOP_MULTI = 0x28,
        READ_DATA = 0x39,
        WRITE_DATA = 0x49,
        LOCK_UNLOCK = 0x82,
        KILL = 0x65,
        SET_RF_CHANNEL = 0xab,
        GET_RF_CHANNEL = 0xaa,
        SET_POWER = 0xb6,
        GET_POWER = 0xb7,
        SET_FHSS = 0xad,
        SET_CW = 0xb0,
        SET_MODEM_PARA = 0xf0,
        READ_MODEM_PARA = 0xf1,
        SET_SELECT_PARA = 0x0c,
        GET_SELECT_PARA = 0x0b,
        SET_INVENTORY_MODE = 0x12,
        SCAN_JAMMER = 0xf2,
        SCAN_RSSI = 0xf3,
        IO_CONTROL = 0x1a,
        RESTART = 0x19,
        SET_READER_ENV_MODE = 0xf5,
        INSERT_FHSS_CHANNEL = 0xa9,
        SLEEP_MODE = 0x17,
        SET_SLEEP_TIME = 0x1d,
        LOAD_NV_CONFIG = 0x0a,
        SAVE_NV_CONFIG = 0x09,
        NXP_CHANGE_CONFIG = 0xe0,
        NXP_READPROTECT = 0xe1,
        NXP_RESET_READPROTECT = 0xe2,
        NXP_CHANGE_EAS = 0xe3,
        NXP_EAS_ALARM = 0xe4,
        IPJ_MONZA_QT_READ = 0xe5,
        IPJ_MONZA_QT_WRITE = 0xe6,
        EXE_FAILED = 0xff
    }
}
