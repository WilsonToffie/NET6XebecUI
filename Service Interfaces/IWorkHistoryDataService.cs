using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Service_Interfaces
{
    public interface IWorkHistoryDataService
    {
        //api/WorkHistory/all/{userId}
        Task<List<WorkHistory>> GetWorkHistories(int appUserId);
    }
}