namespace firmware.Command
{
    public class FrequencyHoppingCommand : BaseCommand
    {
        public FrequencyHoppingCommand(bool enable) : base(1)
        {
            CommandType = CommandTypeEnum.SET_FHSS;

            throw new NotImplementedException();
        }
    }
}
