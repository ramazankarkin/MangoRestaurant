using static Mango.Services.ProductAPI.SD;

namespace Mango.WEB.Models
{
    public class ApiRequest
    {
        public APIType APIType { get; set; } = APIType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }


    }
}
