using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Service_Interfaces
{
    public interface IEducationDataService
    {
        //api/Education/all/{userId}
        Task<List<Education>> GetEducations(int appUserId);
    }
}