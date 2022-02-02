using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Service_Interfaces
{
    public interface IAdditionalInformationDataService
    {
        
        //api/AdditionalInformation/{appUserId}
        Task<AdditionalInformation> GetAdditionalInfo(int appUserId);
    }
}