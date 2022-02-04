using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public interface IApplicationPhaseHelperDataService
    {
        Task<List<ApplicationPhaseHelper>> GetAllApplicationPhaseHelpers();
        List<ApplicationPhaseHelper> GetApplicationPhaseHelpersByUserId(int appUserId);
        List<ApplicationPhaseHelper> GetAssApplicationPhaseHelpers(Applicant applicant, List<Applicant> applicants);
        Task<List<ApplicationPhaseHelper>> UpdatePhaseHelper(ApplicationPhaseHelper applicationPhaseHelper);

         List<ApplicationPhaseHelper> GetLatestHelpers(List<ApplicationPhaseHelper> unFilteredHelpers);


         ApplicationPhaseHelper GetEmptyHelper();
         ApplicationPhaseHelper GetLatestHelper(List<ApplicationPhaseHelper> applicationPhaseHelpers);
    }
}