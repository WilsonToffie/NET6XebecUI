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
        public async Task<IEnumerable<Application>> GetAllApplications()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Application>>
                (await _httpClient.GetStreamAsync($"api/application/all-jobs"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            //var mockApplications = new MockApplicationDataService();
            //return (await mockApplications.GetAllApplications()).ToList();
        }

        //api/Application/{applicationId}
        public async Task<Application> GetApplicationById(int applicationId)
        {
            
            var responseStream = await _httpClient.GetStreamAsync($"api/Application/{applicationId}");
            Application application = await JsonSerializer.DeserializeAsync<Application>(responseStream, _options);
            return application;
        }

        public Task<Application> AddApplication(Application application)
        {
            throw new NotImplementedException();
        }

        public Task UpdateApplication(Application application)
        {
            throw new NotImplementedException();
        }

        public Task DeleteApplication(int applicationId)
        {
            throw new NotImplementedException();
        }
    }
}