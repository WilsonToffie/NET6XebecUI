using System.Threading.Tasks;
using XebecPortal.UI.Pages.Applicant;
using PersonalInformation = XebecPortal.UI.Services.Models.PersonalInformation;

namespace XebecPortal.UI.Service_Interfaces
{
    public interface IPersonalInformationDataService
    {
        
        //api/PersonalInformation/single/{id}
        Task<PersonalInformation> GetPersonalDetails(int appUserId);
        Task<ProfilePortfolioLink> GetPortfolio(int appUserId);
    }
}