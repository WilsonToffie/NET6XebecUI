using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.MockServices;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class ApplicationPhaseHelperDataService : IApplicationPhaseHelperDataService
    {
        private readonly HttpClient _httpClient;
        private HttpClient AltClient = new HttpClient();
        public ApplicationPhaseHelperDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        

        public async Task<List<ApplicationPhaseHelper>> GetAllApplicationPhaseHelpers()
        {
            var phaseHelpers = await JsonSerializer.DeserializeAsync<List<ApplicationPhaseHelper>>(
                utf8Json: await AltClient.GetStreamAsync($"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper"),
                options: new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            return phaseHelpers;
        }
        //api/ApplicationPhaseHelper/UserId={AppUserId}
        public List<ApplicationPhaseHelper> GetApplicationPhaseHelpersByUserId(int appUserId)
        {
            Console.WriteLine($">>>>>ApplicationPhaseHelperDataService : getting helper for {appUserId}");
             return Task.FromResult((AltClient.GetFromJsonAsync<List<ApplicationPhaseHelper>>(
                 $"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/userId={appUserId}"))).Result.Result;
        }

        public List<ApplicationPhaseHelper> GetAssApplicationPhaseHelpers(Applicant applicant, List<Applicant> applicants)
        {
            return AMockDataHub.GetAssApplicationPhaseHelpers(applicant, applicants);
        }
        
    }
}