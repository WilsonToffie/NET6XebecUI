using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public interface IApplicationDataService
    {
        Task<IEnumerable<ApplicationModel>> GetAllApplications();
        Task<ApplicationModel> GetApplicationById(int applicationId);
        Task<ApplicationModel> AddApplication(ApplicationModel applicationModel);
        Task UpdateApplication(ApplicationModel applicationModel);
        Task DeleteApplication(int applicationId);
    }
}