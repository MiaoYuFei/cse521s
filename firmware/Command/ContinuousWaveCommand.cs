namespace firmware.Command
{
    public class ContinuousWaveCommand : BaseCommand
    {
        public ContinuousWaveCommand(bool enable) : base(1)
        {
            CommandType = CommandTypeEnum.SET_CW;

            throw new NotImplementedException();
        }
    }
}
