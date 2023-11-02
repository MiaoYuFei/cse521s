namespace firmware.Command
{
    public class GetRegionCommand : BaseCommand
    {
        public GetRegionCommand() : base(0)
        {
            CommandType = CommandTypeEnum.GET_REGION;
        }
    }
}
