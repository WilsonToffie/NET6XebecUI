using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public interface IApplicationPhaseHelperDataService
    {
        List<ApplicationPhaseHelper> GetAllApplicationPhaseHelpers();
        Task<ApplicationPhaseHelper> GetApplicationPhaseHelperById(int applicationPhaseHelperId);
        Task<ApplicationPhaseHelper> AddApplicationPhaseHelper(ApplicationPhaseHelper applicationPhaseHelper);
        Task UpdateApplicationPhaseHelper(ApplicationPhaseHelper applicationPhaseHelper);
        Task DeleteApplicationPhaseHelper(int applicationPhaseHelperId);
    }
}