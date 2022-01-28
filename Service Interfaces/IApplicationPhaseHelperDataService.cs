using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public interface IApplicationPhaseHelperDataService
    {
        List<ApplicationPhaseHelper> GetAllApplicationPhaseHelpers();
        Task<IEnumerable<ApplicationPhaseHelper>> GetApplicationPhaseHelpersByUserId(int appUserId);
       
    }
}