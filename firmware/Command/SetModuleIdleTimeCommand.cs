namespace firmware.Command
{
    public class SetModuleIdleTimeCommand : BaseCommand
    {
        public SetModuleIdleTimeCommand(byte idleTime) : base(1)
        {
            if (idleTime > 9)
            {
                throw new ArgumentException();
            }

            CommandType = CommandTypeEnum.SET_SLEEP_TIME;

            throw new NotImplementedException();
        }
    }
}
