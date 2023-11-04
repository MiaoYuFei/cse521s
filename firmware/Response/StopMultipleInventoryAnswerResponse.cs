namespace firmware.Response
{
    public class StopMultipleInventoryAnswerResponse : BaseResponse
    {

        public bool Success
        {
            get => Parameter[0] == 0x0 ? true : false;
        }

        public StopMultipleInventoryAnswerResponse(BaseResponse response) : base(response)
        { }
    }
}
