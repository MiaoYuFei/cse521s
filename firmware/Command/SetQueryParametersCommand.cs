namespace firmware.Command
{
    public class SetQueryParametersCommand : BaseCommand
    {

        public SetQueryParametersCommand(byte[] parameter) : base((ushort)parameter.Length)
        {
            CommandType = CommandTypeEnum.SET_QUERY;

            Array.Copy(parameter, 0, Parameter, 0, (ushort)parameter.Length);
        }
    }
}
