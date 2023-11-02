namespace firmware.Command
{
    public class GetChannelCommand : BaseCommand
    {
        public GetChannelCommand() : base(0)
        {
            CommandType = CommandTypeEnum.GET_RF_CHANNEL;
        }
    }
}
