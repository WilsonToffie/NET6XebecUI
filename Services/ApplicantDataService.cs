using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.MockServices;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class ApplicantDataService : IApplicantDataService
    {
        private readonly HttpClient _httpClient;
        private readonly MockApplicantDataService _mocks;

        public ApplicantDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _mocks = new MockApplicantDataService();
        }

        public async Task<IEnumerable<Applicant>> GetAllApplicants()
        {
            // return await JsonSerializer.DeserializeAsync<IEnumerable<Applicant>>
            //     (await _httpClient.GetStreamAsync($"api/applicant/all-jobs"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return (await _mocks.GetAllApplicants()).ToList();
        }

        public async Task<IEnumerable<Applicant>> GetAllApplicantsByJobId(int jobId)
        {
            // return await JsonSerializer.DeserializeAsync<IEnumerable<Applicant>>
            // (await _httpClient.GetStreamAsync($"api/applicant/{jobId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            var mocks = new MockApplicantDataService();
            return await mocks.GetAllApplicants();
        }

        public async Task<Applicant> GetApplicantDetails(int applicantId)
        {
            return await JsonSerializer.DeserializeAsync<Applicant>
            (await _httpClient.GetStreamAsync($"api/applicant/{applicantId}"),
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
        }

        public async Task<Applicant> AddApplicant(Applicant applicant)
        {
            var applicantJson =
                new StringContent(JsonSerializer.Serialize(applicant), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/applicant", applicantJson);

            if (response.IsSuccessStatusCode)
                return await JsonSerializer.DeserializeAsync<Applicant>(await response.Content.ReadAsStreamAsync());

            return null;
        }

        public async Task UpdateApplicant(Applicant applicant)
        {
            // var applicantJson =
            //     new StringContent(JsonSerializer.Serialize(applicant), Encoding.UTF8, "application/json");
            //
            // await _httpClient.PutAsync("api/applicant", applicantJson);

            await _mocks.UpdateApplicant(applicant);
        }

        public async Task DeleteApplicant(int applicantId)
        {
            await _httpClient.DeleteAsync($"api/applicant/{applicantId}");
        }
    }
}