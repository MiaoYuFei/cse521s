namespace firmware.Command
{
    public class ScanJammerCommand : BaseCommand
    {
        public ScanJammerCommand() : base(0)
        {
            CommandType = CommandTypeEnum.SCAN_JAMMER;
        }
    }
}
