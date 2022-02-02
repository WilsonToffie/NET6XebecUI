using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Service_Interfaces
{
    public interface IPhaseDataService
    {
        //api/Status
        Task<List<AppPhase>> GetApplicationPhases();
        //api/Status/{id}
        Task<AppPhase> GeApplicationPhaseById(int id);
    }
}