using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public interface IApplicationDataService
    {
        Task<IEnumerable<Application>> GetAllApplications();
        Task<Application> GetApplicationById(int applicationId);
        Task<Application> AddApplication(Application application);
        Task UpdateApplication(Application application);
        Task DeleteApplication(int applicationId);
    }
}