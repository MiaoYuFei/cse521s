using System.Buffers.Binary;

namespace firmware.Response
{
    public class GetTXPowerAnswerResponse : BaseResponse
    {
        public ushort Power
        {
            get {
                byte[] powerBytes = new byte[sizeof(ushort)];
                Array.Copy(Parameter, 0, powerBytes, 0, sizeof(ushort));
                return (ushort)(BinaryPrimitives.ReadUInt16BigEndian(powerBytes) / 100);
            }
        }

        public GetTXPowerAnswerResponse(BaseResponse response) : base(response)
        {
            if (response.CommandType != CommandTypeEnum.GET_POWER)
            {
                throw new ArgumentException();
            }
        }
    }
}
