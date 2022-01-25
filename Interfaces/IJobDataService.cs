using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public interface IJobDataService
    {
        Task<IEnumerable<Job>> GetAllJobs();
        Task<Job> GetJobDetails(int jobId);
        Task<Job> AddJob(Job job);
        Task UpdateJob(Job job);
        Task DeleteJob(int jobId);
    }
}