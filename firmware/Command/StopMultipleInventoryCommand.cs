namespace firmware.Command
{
    public class StopMultipleInventoryCommand : BaseCommand
    {
        public StopMultipleInventoryCommand() : base(0)
        {
            CommandType = CommandTypeEnum.STOP_MULTI;
        }
    }
}
