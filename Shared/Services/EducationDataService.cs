using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Service_Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class EducationDataService : IEducationDataService
    {
        private readonly HttpClient _httpClient;
        private HttpClient altClient = new HttpClient();
        public EducationDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    
        
        //api/Education/all/{userId}
        public async Task<List<Education>> GetEducations(int appUserId)
        {
            return await JsonSerializer.DeserializeAsync<List<Education>>
                (await altClient.GetStreamAsync($"Education/all/{appUserId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        }
    }
}