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
            var screening = new AppPhase
            {
                Id = 1,
                Description = "Screening",
                
            };
            var application = new AppPhase
            {
                Id = 5,
                Description = "Application",
                
            };
            var interviewHr = new AppPhase
            {
                Id = 3,
                Description = "Interview - HR",
               
            };
            var interviewStaff = new AppPhase
            {
                Id = 4,
                Description = "Interview - Staff",
                
            };
            var testing = new AppPhase
            {
                Id = 2,
                Description = "Testing",
               
            };
            
            var offer = new AppPhase
            {
                Id = 25,
                Description = "Offer",
                
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
//"id": 1,
//"description": "Screening"
//"id": 2,
//"description": "Code Submission",
//"id": 3,
//"description": "Interview",
//"id": 5,
//"description": "Applied",
//"id": 25,
//"description": "Offer",