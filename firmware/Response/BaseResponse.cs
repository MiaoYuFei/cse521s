using System.Buffers.Binary;
namespace firmware.Response
{
    public class BaseResponse
    {
        public static byte Header { get => (byte)ConstantEnum.HeaderFlag; }

        public FrameTypeEnum FrameType { get; private set; }

        public CommandTypeEnum CommandType { get; private set; }

        public ushort ParameterLength { get; private set; }

        public byte[] Parameter { get; private set; }

        public static byte End { get => (byte)ConstantEnum.EndingFlag; }

        public BaseResponse(BaseResponse @object)
        {
            FrameType = @object.FrameType;
            CommandType = @object.CommandType;
            ParameterLength = @object.ParameterLength;
            Parameter = @object.Parameter;
        }

        public BaseResponse(byte[] data)
        {
            using MemoryStream ms = new(data);
            using BinaryReader br = new(ms);

            if (Header != br.ReadByte())
            {
                throw new ArgumentException();
            }

            FrameType = (FrameTypeEnum)br.ReadByte();

            CommandType = (CommandTypeEnum)br.ReadByte();

            ParameterLength = BinaryPrimitives.ReadUInt16BigEndian(br.ReadBytes(sizeof(ushort)));

            Parameter = new byte[ParameterLength];

            Array.Copy(br.ReadBytes(ParameterLength), 0, Parameter, 0, ParameterLength);

            int checksum = 0;
            checksum += (int)FrameType;
            checksum += (int)CommandType;
            checksum += (int)(ParameterLength >> 8);
            checksum += (int)(ParameterLength & 0xff);
            for (int i = 0; i < ParameterLength; i++)
            {
                checksum += (int)Parameter[i];
            }
            if ((byte)(checksum & 0xff) != br.ReadByte())
            {
                throw new ArgumentException();
            }

            if (End != br.ReadByte())
            {
                throw new ArgumentException();
            }
        }
    }
}
