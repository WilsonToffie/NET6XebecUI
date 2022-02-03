using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Service_Interfaces
{
    public interface IPhaseDataService
    {
        //api/Status
        Task<List<PhaseModel>> GetApplicationPhases();
        //api/Status/{id}
        Task<PhaseModel> GeApplicationPhaseById(int id);
    }
}