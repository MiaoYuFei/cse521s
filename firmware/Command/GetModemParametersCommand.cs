namespace firmware.Command
{
    public class GetModemParametersCommand : BaseCommand
    {
        public GetModemParametersCommand() : base(0)
        {
            CommandType = CommandTypeEnum.READ_MODEM_PARA;

            throw new NotImplementedException();
        }
    }
}
