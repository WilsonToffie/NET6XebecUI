using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public class ApplicationPhaseHelperDataService : IApplicationPhaseHelperDataService
    {
        private readonly HttpClient _httpClient;

        public ApplicationPhaseHelperDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public List<ApplicationPhaseHelper> GetAllApplicationPhaseHelpers()
        {
            throw new NotImplementedException();
        }
        //api/ApplicationPhaseHelper/UserId={AppUserId}
        public async Task<IEnumerable<ApplicationPhaseHelper>> GetApplicationPhaseHelpersByUserId(int appUserId)
        {
            var val = await JsonSerializer.DeserializeAsync<IEnumerable<ApplicationPhaseHelper>>(
                utf8Json: await _httpClient.GetStreamAsync($"api/ApplicationPhaseHelper/UserId={appUserId}"),
                options: new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                

            return val;
        }
        
    }
}