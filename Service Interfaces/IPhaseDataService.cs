using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Service_Interfaces
{
    public interface IPhaseDataService
    {
        //api/Status
        List<AppPhase> GetApplicationPhases();
        //api/Status/{id}
        AppPhase GeApplicationPhaseById(int id);
    }
}