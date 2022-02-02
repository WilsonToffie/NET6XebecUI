using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Service_Interfaces
{
    public interface IPersonalInformationDataService
    {
        
        //api/PersonalInformation/single/{id}
        Task<PersonalInformation> GetPersonalDetails(int appUserId);
    }
}