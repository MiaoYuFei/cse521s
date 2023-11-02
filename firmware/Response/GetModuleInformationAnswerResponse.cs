using System.Text;
using static firmware.Command.GetModuleInformationCommand;

namespace firmware.Response
{
    public class GetModuleInformationAnswerResponse : BaseResponse
    {
        public ModuleInfoTypeEnum ModuleInfoType
        {
            get => (ModuleInfoTypeEnum)Parameter[0];
        }

        public string Info
        {
            get => Encoding.ASCII.GetString(Parameter, 1, Parameter.Length - 1);
        }

        public GetModuleInformationAnswerResponse(BaseResponse response) : base(response)
        {
            if (response.CommandType != CommandTypeEnum.GET_MODULE_INFO)
            {
                throw new ArgumentException();
            }
        }
    }
}
