using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Service_Interfaces
{
    public interface IApplicationPhaseHelperDataService
    {
        Task<List<ApplicationPhaseHelper>> GetAllApplicationPhaseHelpers();
        Task<List<ApplicationPhaseHelper>> GetApplicationPhaseHelpersByUserId(int appUserId);
        Task<List<ApplicationPhaseHelper>> GetAssApplicationPhaseHelpers(Applicant applicant,
            List<Applicant> applicants);

        Task UpdateApplicationPhaseHelper(ApplicationPhaseHelper applicationPhaseHelper);
        Task<ApplicationPhaseHelper> GetApplicationPhaseHelperByUserId(int appUserId);

        ApplicationPhaseHelper GetEmptyHelper();
    }
}