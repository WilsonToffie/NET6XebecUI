using System.Collections.Generic;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public interface IMyJobListDataService
    {
        List<MyJob> GetAllJobs();
        List<MyJob> GetAllJobsByAppUserId(int appUserId);
    }
}