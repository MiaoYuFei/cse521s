namespace firmware.Response
{
    public class InventoryNoticeResponse : BaseResponse
    {
        public sbyte RSSI
        {
            get => (sbyte)Parameter[0];
        }

        public byte[] PC
        {
            get
            {
                byte[] bytes = new byte[2];
                Array.Copy(Parameter, 1, bytes, 0, 2);

                return bytes;
            }
        }

        public byte[] EPC
        {
            get
            {
                byte[] bytes = new byte[12];
                Array.Copy(Parameter, 3, bytes, 0, 12);

                return bytes;
            }
        }

        public byte[] CRC
        {
            get
            {
                byte[] bytes = new byte[2];
                Array.Copy(Parameter, 15, bytes, 0, 2);

                return bytes;
            }
        }

        public InventoryNoticeResponse(BaseResponse response) : base(response)
        {
            if (response.CommandType != CommandTypeEnum.INVENTORY)
            {
                throw new ArgumentException();
            }
        }
    }
}
