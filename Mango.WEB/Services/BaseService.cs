using Mango.WEB.Models;
using Mango.WEB.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Mango.WEB.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDTO responseModel { get; set; }

        public IHttpClientFactory _httpClient { get; set; } // Request yaptığımızda kullanıcaz.

        public BaseService(IHttpClientFactory httpClient)
        {
            responseModel = new ResponseDTO();
            _httpClient = httpClient;
        }


        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("MangoAPI");
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Headers.Add("Accept","application/json");
                httpRequestMessage.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if(apiRequest.Data != null)
                {
                    httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                };

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);// Sending a token each request.
                }
                HttpResponseMessage apiResponse = null;
                switch (apiRequest.APIType)
                {
                    case SD.APIType.POST:
                        httpRequestMessage.Method = HttpMethod.Post;
                        break;
                    case SD.APIType.PUT:
                        httpRequestMessage.Method = HttpMethod.Put;
                        break;
                    case SD.APIType.DELETE:
                        httpRequestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        httpRequestMessage.Method = HttpMethod.Get;
                        break;
                }
                apiResponse = await client.SendAsync(httpRequestMessage);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent); // hangi tipte obje gelirse o tipe dönüştürüyoruz.
                return apiResponseDto; // genelde T olarak ResponseDTO göndereceğimiz için dönüş tipimiz ResponseDTO olacak.
            }
            catch (Exception ex)
            {

                var dto = new ResponseDTO
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string>
                    {
                        Convert.ToString(ex.Message)
                    },
                    IsSUCCESS = false
                };

                var res = JsonConvert.SerializeObject(dto); // objeyi json'a convert ettik.
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res); //json'ı generic tipe convert ettik.
                return apiResponseDto;
            }
        }


        public void Dispose()
        {
            GC.SuppressFinalize(true); // Ne için kullanıldığına bak tekrardan!
        }
    }
}
