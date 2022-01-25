using System.Collections.Generic;
using System.Linq;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.MockServices;

namespace XebecPortal.UI.Services.Models
{
    public class MyJobListDataService : IMyJobListDataService
    {
        public List<MyJob> GetAllJobs()
        {
            var mocks = new MockMyJobListDataService();
            return  mocks.GetAllJobs().ToList();
        }

        public List<MyJob> GetAllJobsByAppUserId(int appUserId)
        {
            throw new System.NotImplementedException();
        }
    }
}