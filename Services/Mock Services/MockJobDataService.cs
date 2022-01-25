using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services.MockServices
{
    public class MockJobDataService : IJobDataService
    {
        private List<Job> _jobs;

        private IEnumerable<Job> Jobs
        {
            get
            {
                if (_jobs == null)
                    InitializeJobs();
                return _jobs;
            }
        }


        public async Task<IEnumerable<Job>> GetAllJobs()
        {
            return await Task.Run(() => Jobs);
        }

        public Task<Job> GetJobDetails(int jobId)
        {
            throw new NotImplementedException();
        }

        public Task<Job> AddJob(Job job)
        {
            throw new NotImplementedException();
        }

        public Task UpdateJob(Job job)
        {
            throw new NotImplementedException();
        }

        public Task DeleteJob(int jobId)
        {
            throw new NotImplementedException();
        }

        private void InitializeJobs()
        {
            if (_jobs != null) return;
            if (_jobs == null) _jobs = AMockDataHub.MockJobs.ToList();
        }
    }
}