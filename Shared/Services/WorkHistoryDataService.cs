using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Service_Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class WorkHistoryDataService : IWorkHistoryDataService
    {
        private readonly HttpClient _httpClient;
        private HttpClient altClient = new HttpClient();
        public WorkHistoryDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    
        
        //api/WorkHistory/all/{appUserId}
        public async Task<List<WorkHistory>> GetWorkHistories(int appUserId)
        {
            return await JsonSerializer.DeserializeAsync<List<WorkHistory>>
                (await altClient.GetStreamAsync($"WorkHistory/all/{appUserId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}