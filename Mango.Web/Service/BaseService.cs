using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDTO?> SendAsync(RequestDTO request)
        {
            HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            // TODO - Token
            message.RequestUri = new Uri(request.Url);
            if (request.Data is not null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
            }

            message.Method = request.ApiType.ToMethod();
            HttpResponseMessage? apiResponse = await client.SendAsync(message);

            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access Denied" };
                case HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Not Authorized" };
                case HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Interna lServer Error" };
                default:
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                    return apiResponseDto;
            }
        }
    }
}
