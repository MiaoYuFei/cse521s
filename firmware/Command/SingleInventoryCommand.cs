namespace firmware.Command
{
    public class SingleInventoryCommand : BaseCommand
    {
        public SingleInventoryCommand() : base(0)
        {
            CommandType = CommandTypeEnum.INVENTORY;
        }
    }
}
