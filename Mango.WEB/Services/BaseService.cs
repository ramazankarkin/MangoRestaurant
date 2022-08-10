﻿using Mango.WEB.Models;
using Mango.WEB.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace Mango.WEB.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDTO responseModel { get; set; }

        public IHttpClientFactory httpClient { get; set; } // Request yaptığımızda kullanıcaz.

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new ResponseDTO();
            this.httpClient = httpClient;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(true); // Ne için kullanıldığına bak tekrardan!
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MangoAPI");
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Headers.Add("Accept","application/json");
                httpRequestMessage.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if(apiRequest.Data != null)
                {
                    httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                };
                HttpResponseMessage apiResponse = null;
                switch (apiRequest.APIType)
                {
                    case Mango.Services.ProductAPI.SD.APIType.POST:
                        httpRequestMessage.Method = HttpMethod.Post;
                        break;
                    case Mango.Services.ProductAPI.SD.APIType.PUT:
                        httpRequestMessage.Method = HttpMethod.Put;
                        break;
                    case Mango.Services.ProductAPI.SD.APIType.DELETE:
                        httpRequestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        httpRequestMessage.Method = HttpMethod.Get;
                        break;
                }
                apiResponse = await client.SendAsync(httpRequestMessage);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDto;
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

                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDto;
            }
        }
    }
}
