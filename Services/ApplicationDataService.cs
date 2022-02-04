using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.MockServices;
using XebecPortal.UI.Services.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace XebecPortal.UI.Services
{
    public class ApplicationDataService : IApplicationDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        

        public ApplicationDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

           
        }
        public async Task<IEnumerable<ApplicationModel>> GetAllApplications()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<ApplicationModel>>
                (await _httpClient.GetStreamAsync($"api/applicationModel/all-jobs"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            //var mockApplications = new MockApplicationDataService();
            //return (await mockApplications.GetAllApplications()).ToList();
        }

        //api/ApplicationModel/{applicationId}
        public async Task<ApplicationModel> GetApplicationById(int applicationId)
        {
            
            var responseStream = await _httpClient.GetStreamAsync($"api/ApplicationModel/{applicationId}");
            ApplicationModel applicationModel = await JsonSerializer.DeserializeAsync<ApplicationModel>(responseStream, _options);
            return applicationModel;
        }

        public Task<ApplicationModel> AddApplication(ApplicationModel applicationModel)
        {
            throw new NotImplementedException();
        }

        public Task UpdateApplication(ApplicationModel applicationModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteApplication(int applicationId)
        {
            throw new NotImplementedException();
        }
    }
}