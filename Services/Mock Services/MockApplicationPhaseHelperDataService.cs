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
        public async Task<List<ApplicationPhaseHelper>> GetAllApplicationPhaseHelpers()
        {
            return AMockDataHub.MockApplicationHelpers.ToList();
        }

        //api/ApplicationPhaseHelper/UserId={AppUserId}
        public Task<List<ApplicationPhaseHelper>> GetApplicationPhaseHelpersByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}