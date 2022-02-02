using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Service_Interfaces
{
    public interface IStatusDataService
    {
        //api/ApplicationPhase
        Task<List<AppStatus>> GetAllStatuses();
        //api/ApplicationPhase/{id}
        Task<AppStatus> GetStatusById(int id);
    }
}