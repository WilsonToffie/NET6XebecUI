using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services.MockServices
{
    public class MockApplicationPhaseHelperDataService : IApplicationPhaseHelperDataService
    {
        public List<ApplicationPhaseHelper> GetAllApplicationPhaseHelpers()
        {
            return AMockDataHub.MockApplicationHelpers.ToList();
        }

        public Task<ApplicationPhaseHelper> GetApplicationPhaseHelperById(int applicationPhaseHelperId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationPhaseHelper> AddApplicationPhaseHelper(ApplicationPhaseHelper applicationPhaseHelper)
        {
            throw new NotImplementedException();
        }

        public Task UpdateApplicationPhaseHelper(ApplicationPhaseHelper applicationPhaseHelper)
        {
            throw new NotImplementedException();
        }

        public Task DeleteApplicationPhaseHelper(int applicationPhaseHelperId)
        {
            throw new NotImplementedException();
        }
    }
}