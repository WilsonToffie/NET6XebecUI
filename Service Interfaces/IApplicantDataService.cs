using System.Collections.Generic;
using System.Threading.Tasks;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Interfaces
{
    public interface IApplicantDataService
    {
        Task<IEnumerable<Applicant>> GetAllApplicants();
        public Task<IEnumerable<Applicant>> GetAllApplicantsByJobId(int jobId);
        Task<Applicant> GetApplicantDetails(int applicantId);
        Task<Applicant> AddApplicant(Applicant applicant);
        Task UpdateApplicant(Applicant applicant);
        Task DeleteApplicant(int applicantId);
    }
}