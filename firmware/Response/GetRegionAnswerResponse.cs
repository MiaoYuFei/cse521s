using static firmware.Command.SetRegionCommand;

namespace firmware.Response
{
    public class GetRegionAnswerResponse : BaseResponse
    {
        public RegionCodeEnum RegionCode
        {
            get {
                return (RegionCodeEnum)Parameter[0];
            }
        }

        public GetRegionAnswerResponse(BaseResponse response) : base(response)
        {
            if (response.CommandType != CommandTypeEnum.GET_REGION)
            {
                throw new ArgumentException();
            }
        }
    }
}
