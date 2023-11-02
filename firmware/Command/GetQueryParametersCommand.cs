namespace firmware.Command
{
    public class GetQueryParametersCommand : BaseCommand
    {
        public GetQueryParametersCommand() : base(0)
        {
            CommandType = CommandTypeEnum.GET_QUERY;
        }
    }
}
