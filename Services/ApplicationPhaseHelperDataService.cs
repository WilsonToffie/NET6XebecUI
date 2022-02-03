using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using XebecPortal.UI.Interfaces;
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
            
            // return (await JsonSerializer.DeserializeAsync<List<ApplicationPhaseHelper>>
            //     (await AltClient.GetStreamAsync(
            //         $"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/userId={appUserId}"),
            //     new JsonSerializerOptions()
            //     {
            //         PropertyNameCaseInsensitive = true
            //     }));
            Console.WriteLine($">>>>>ApplicationPhaseHelperDataService : getting helper for {appUserId}");
             return Task.FromResult((AltClient.GetFromJsonAsync<List<ApplicationPhaseHelper>>(
                 $"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/userId={appUserId}"))).Result.Result;
        }

        // List<ApplicationPhaseHelper> GetMockApplicationHelper(List<Applicant> applicants)
        // {
        //     //set application list - Job (mock job list)
        //     //create application list -
        //     //  appuser -for each pick random applicant Id
        //     throw NotImplementedException;
        // }
        //
        // public List<JobModel> GetMockJobs(int num)
        // {
        //     var faker = new Faker<JobModel>()
        //         .RuleFor();
        // }
    }
}