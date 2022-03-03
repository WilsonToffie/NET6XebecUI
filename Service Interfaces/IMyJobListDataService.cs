using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public interface IMyJobListDataService
    {
        List<MyJob> GetAllJobs();
        Task<List<MyJob>> GetAllJobsByAppUserId(int appUserId);
    }
}