using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XebecPortal.UI.Service_Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class StatusDataService : IStatusDataService
    {
        private readonly HttpClient _httpClient;
        private HttpClient altClient = new HttpClient();
        
        private List<AppStatus> _appStatuses;
        private List<AppStatus> AppStatuses
        {
            get
            {
                if (_appStatuses == null)
                    IntializeStatus();
                return _appStatuses;
            }
        }

        public StatusDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }

        private void IntializeStatus()
        {
            var progress = new AppStatus
            {
                Id = 1,
                Description = "In Progress",
                StatusEnum = StatusEnum.InProgress
            };
            var rejected = new AppStatus
            {
                Id = 2,
                Description = "Rejected",
                StatusEnum = StatusEnum.Rejected
            };
            var statuses = new List<AppStatus> {progress, rejected};
            _appStatuses = statuses;
        }

        public async Task<List<AppStatus>> GetAllStatuses()
        {
            return AppStatuses;
        }

        public async Task<AppStatus> GetStatusById(int id)
        {
            return AppStatuses.FirstOrDefault(a => a.Id == id);
        }
        
    }
}