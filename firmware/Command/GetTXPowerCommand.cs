namespace firmware.Command
{
    public class GetTXPowerCommand : BaseCommand
    {
        public GetTXPowerCommand() : base(0)
        {
            CommandType = CommandTypeEnum.GET_POWER;
        }
    }
}
