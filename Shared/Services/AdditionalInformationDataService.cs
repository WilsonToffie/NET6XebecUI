using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Service_Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class AdditionalInformationDataService : IAdditionalInformationDataService
    {
        private readonly HttpClient _httpClient;
        private HttpClient altClient = new HttpClient();
        public AdditionalInformationDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    
        
        //api/AdditionalInformation/{appUserId}
        public async Task<AdditionalInformation> GetAdditionalInfo(int appUserId)
        {
            return await JsonSerializer.DeserializeAsync<AdditionalInformation>
            (await altClient.GetStreamAsync(
                    $"https://xebecapi.azurewebsites.net/api/AdditionalInformation/{appUserId}"),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}