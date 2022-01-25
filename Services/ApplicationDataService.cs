using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.MockServices;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class ApplicationDataService : IApplicationDataService
    {
        public async Task<IEnumerable<Application>> GetAllApplications()
        {
            // return await JsonSerializer.DeserializeAsync<IEnumerable<Applicant>>
            //     (await _httpClient.GetStreamAsync($"api/applicant/all-jobs"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            var mockApplications = new MockApplicationDataService();
            return (await mockApplications.GetAllApplications()).ToList();
        }

        public Task<Application> GetApplicationById(int applicationId)
        {
            throw new NotImplementedException();
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