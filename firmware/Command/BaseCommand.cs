using firmware.Response;
using System.Buffers.Binary;

namespace firmware.Command
{
    public abstract class BaseCommand
    {
        public static byte Header { get => (byte)ConstantEnum.HeaderFlag; }

        public static FrameTypeEnum FrameType { get => FrameTypeEnum.COMMAND; }

        public CommandTypeEnum CommandType { get; internal set; }

        public ushort ParameterLength { get; private set; }

        public byte[] Parameter { get; private set; }

        public byte CheckSum {
            get {
                int checksum = 0;
                checksum += (int)FrameType;
                checksum += (int)CommandType;
                checksum += (int)(ParameterLength >> 8);
                checksum += (int)(ParameterLength & 0xff);
                for (int i = 0; i < ParameterLength; i++)
                {
                    checksum += (int)Parameter[i];
                }

                return (byte)(checksum & 0xff);
            }
        }

        public static byte End { get => (byte)ConstantEnum.EndingFlag; }

        public BaseCommand(ushort parameterLength)
        {
            ParameterLength = parameterLength;
            Parameter = new byte[ParameterLength];
        }

        public byte[] GetBytes()
        {
            using MemoryStream ms = new();
            using (BinaryWriter bw = new(ms))
            {
                bw.Write(Header);
                bw.Write((byte)FrameType);
                bw.Write((byte)CommandType);
                Span<byte> parameterLengthBytes = new byte[sizeof(ushort)];
                BinaryPrimitives.WriteUInt16BigEndian(parameterLengthBytes, ParameterLength);
                bw.Write(parameterLengthBytes.ToArray());
                bw.Write(Parameter);
                bw.Write(CheckSum);
                bw.Write(End);
            }

            return ms.ToArray();
        }
    }
}
