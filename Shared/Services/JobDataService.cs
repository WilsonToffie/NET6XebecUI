using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Pages.HR;
using XebecPortal.UI.Services.MockServices;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class JobDataService : IJobDataService
    {
        private readonly HttpClient _httpClient;

        public JobDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Job>> GetAllJobs()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Job>>
            (await _httpClient.GetStreamAsync($"job"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            //var mocks = new MockJobDataService();

            //return await mocks.GetAllJobs();
        }

        public async Task<Job> GetJobDetails(int jobId)
        {
            return await JsonSerializer.DeserializeAsync<Job>
            (await _httpClient.GetStreamAsync($"api/job/{jobId}"),
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
        }

        public async Task<Job> AddJob(Job job)
        {
            var jobJson =
                new StringContent(JsonSerializer.Serialize(job), Encoding.UTF8, "applicationModel/json");

            var response = await _httpClient.PostAsync("api/job", jobJson);

            if (response.IsSuccessStatusCode)
                return await JsonSerializer.DeserializeAsync<Job>(await response.Content.ReadAsStreamAsync());

            return null;
        }

        public async Task UpdateJob(Job job)
        {
            var jobJson =
                new StringContent(JsonSerializer.Serialize(job), Encoding.UTF8, "applicationModel/json");

            await _httpClient.PutAsync("api/job", jobJson);
        }

        

        public async Task DeleteJob(int jobId)
        {
            await _httpClient.DeleteAsync($"api/job/{jobId}");
        }
    }
}