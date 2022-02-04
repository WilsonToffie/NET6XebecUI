using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
                Description = "ApplicationModel",
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

        public PhaseDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public List<AppPhase> GetApplicationPhases()
        {
            return AppPhases;
        }

        public AppPhase GeApplicationPhaseById(int id)
        {
            return AppPhases.FirstOrDefault(a => a.Id == id);
        }
    }
}