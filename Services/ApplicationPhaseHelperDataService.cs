using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public class ApplicationPhaseHelperDataService : IApplicationPhaseHelperDataService
    {
        public List<ApplicationPhaseHelper> GetAllApplicationPhaseHelpers()
        {
            throw new NotImplementedException();
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