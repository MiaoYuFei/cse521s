namespace firmware.Command
{
    public class SetModemParametersCommand : BaseCommand
    {
        public SetModemParametersCommand(byte mixerG, byte ifG, ushort threshold) : base(4)
        {
            CommandType = CommandTypeEnum.SET_MODEM_PARA;

            throw new NotImplementedException();
        }
    }
}
