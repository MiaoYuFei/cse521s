namespace firmware.Command
{
    public class GPIOCommand : BaseCommand
    {

        public enum IOOperationEnum : byte
        {
            SET_IO_DIRECTION = 0x0,
            SET_IO_STATUS = 0x1,
            GET_IO_STATUS = 0x2
        }

        public enum IORangeEnum : byte
        {
            IO1 = 0x1,
            IO2 = 0x2,
            IO3 = 0x3,
            IO4 = 0x4
        }

        public enum IODirectionStatusEnum : byte
        {
            INPUT_OR_LOW = 0x0,
            OUTPUT_OR_HIGH = 0x1
        }

        public GPIOCommand(IOOperationEnum ioOperation, IORangeEnum ioRange, IODirectionStatusEnum ioDirectionStatus) : base(3)
        {
            CommandType = CommandTypeEnum.IO_CONTROL;

            throw new NotImplementedException();
        }
    }
}
