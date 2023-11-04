namespace firmware.Command
{
    public class ModuleSleepCommand : BaseCommand
    {
        public ModuleSleepCommand() : base(0)
        {
            CommandType = CommandTypeEnum.SLEEP_MODE;
        }
    }
}
