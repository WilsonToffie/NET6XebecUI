using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Pages.Applicant;
using XebecPortal.UI.Service_Interfaces;
using PersonalInformation = XebecPortal.UI.Services.Models.PersonalInformation;

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
            return await JsonSerializer.DeserializeAsync<PersonalInformation>
                (await altClient.GetStreamAsync($"https://xebecapi.azurewebsites.net/api/PersonalInformation/single/{appUserId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });   
        }

        public async Task<ProfilePortfolioLink> GetPortfolio(int appUserId)
        {
            return await JsonSerializer.DeserializeAsync<ProfilePortfolioLink>
                (await altClient.GetStreamAsync($"https://xebecapi.azurewebsites.net/api/ProfilePortfolioLink/{appUserId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });   
        }
    }
}