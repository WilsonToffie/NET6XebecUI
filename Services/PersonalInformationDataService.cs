using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Service_Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class PersonalInformationDataService : IPersonalInformationDataService
    {
        private readonly HttpClient _httpClient;
        private HttpClient altClient = new HttpClient();
        public PersonalInformationDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    
        
        //api/PersonalInformation/single/{appUserId}
        public async Task<PersonalInformation> GetPersonalDetails(int appUserId)
        {
            Console.WriteLine($"Getting details for {appUserId}");
            
            return await JsonSerializer.DeserializeAsync<PersonalInformation>
                (await altClient.GetStreamAsync($"https://xebecapi.azurewebsites.net/api/PersonalInformation/single/{appUserId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });   
        }
    }
}