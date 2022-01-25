using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services.MockServices
{
    public class MockApplicationDataService : IApplicationDataService
    {
        private IEnumerable<Application> _applications;

        public IEnumerable<Application> Applications
        {
            get
            {
                if (_applications == null) InitializeApplications();

                return _applications;
            }
        }

        public async Task<IEnumerable<Application>> GetAllApplications()
        {
            return await Task.Run(() => Applications);
        }

        public Task<Application> GetApplicationById(int applicationId)
        {
            throw new NotImplementedException();
        }

        public Task<Application> AddApplication(Application application)
        {
            throw new NotImplementedException();
        }

        public Task UpdateApplication(Application application)
        {
            throw new NotImplementedException();
        }

        public Task DeleteApplication(int applicationId)
        {
            throw new NotImplementedException();
        }

        private void InitializeApplications()
        {
            if (_applications != null)
                return;
            _applications = AMockDataHub.MockApplications.ToList();
        }
    }
}