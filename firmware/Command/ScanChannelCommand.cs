namespace firmware.Command
{
    public class ScanChannelCommand : BaseCommand
    {
        public ScanChannelCommand() : base(0)
        {
            CommandType = CommandTypeEnum.SCAN_RSSI;
        }
    }
}
