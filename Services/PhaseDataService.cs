using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XebecPortal.UI.Service_Interfaces;
using XebecPortal.UI.Services.MockServices;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class PhaseDataService : IPhaseDataService
    {
        private readonly HttpClient _httpClient;
        private HttpClient altClient = new HttpClient();
        
        private List<AppPhase> _appPhases;
        private List<PhaseModel> _applicationPhases;

        private List<AppPhase> AppPhases
        {
            get
            {
                if (_appPhases == null)
                    IntializePhases();
                return _appPhases;
            }
        }

        private void IntializePhases()
        {
             
            var application = new AppPhase
            {
                Id = 1,
                Description = "Application",
                PhaseEnum = PhaseEnum.Application
            };
            var interviewHr = new AppPhase
            {
                Id = 2,
                Description = "Interview - HR",
                PhaseEnum = PhaseEnum.InterviewHr
            };
            var interviewStaff = new AppPhase
            {
                Id = 3,
                Description = "Interview - Staff",
                PhaseEnum = PhaseEnum.InterviewStaff
            };
            var testing = new AppPhase
            {
                Id = 4,
                Description = "Testing",
                PhaseEnum = PhaseEnum.Testing
            };
            var screening = new AppPhase
            {
                Id = 5,
                Description = "Screening",
                PhaseEnum = PhaseEnum.Screening
            };
            var offer = new AppPhase
            {
                Id = 6,
                Description = "Offer",
                PhaseEnum = PhaseEnum.Offer
            };
            _appPhases = new List<AppPhase>{application, interviewHr, interviewStaff, testing, screening, offer};
        }

        public List<PhaseModel> ApplicationPhases
        {
            get => _applicationPhases;
            set => _applicationPhases = value;
        }

        public PhaseDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<PhaseModel>> GetApplicationPhases()
        {
            return await altClient.GetFromJsonAsync<List<PhaseModel>>(
                $"https://xebecapi.azurewebsites.net/api/applicationphase/");
        }

        public async Task<PhaseModel> GeApplicationPhaseById(int id)
        {
            return await altClient.GetFromJsonAsync<PhaseModel>(
                $"https://xebecapi.azurewebsites.net/api/ApplicationPhase/{id}");
        }
        
    }
}