using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Shared.Home.Models;
using XebecPortal.UI.Pages.Applicant.Models;
using XebecPortal.UI.Pages.Applicant;

namespace XebecPortal.UI.Shared
{
    public partial class MainLayout
    {
        private IList<Job> jobs = new List<Job>();
        private IList<JobType> jobTypes = new List<JobType>();

        private bool HRIsHidden, ApplicantIsHidden = true;

        private bool applicantApplicationProfileIsHidden, applicantJobPortalIsHidden, applicantMyJobsIsHidden = true;

        private bool hrDataAnalyticsToolIsHidden, hrJobPortalIsHidden, hrCreateAJobIsHidden = true;

        protected override async Task OnInitializedAsync()
        {
            if (state.Role.Equals("Candidate"))
            {
                ApplicantIsHidden = false;
                HRIsHidden = true;

                applicantApplicationProfileIsHidden = true;
                applicantJobPortalIsHidden = false;
                applicantMyJobsIsHidden = true;

                hrDataAnalyticsToolIsHidden = true;
                hrJobPortalIsHidden = true;
                hrCreateAJobIsHidden = true;


                jobs = await HttpClient.GetFromJsonAsync<IList<Job>>("https://xebecapi.azurewebsites.net/api/Job");
                jobTypes = await HttpClient.GetFromJsonAsync<IList<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");
            }

            else if (state.Role.Equals("HRAdmin"))
            {
                HRIsHidden = false;
                ApplicantIsHidden = true;

                hrDataAnalyticsToolIsHidden = true;
                hrJobPortalIsHidden = false;
                hrCreateAJobIsHidden = true;

                applicantApplicationProfileIsHidden = true;
                applicantJobPortalIsHidden = true;
                applicantMyJobsIsHidden = true;
            }
            await Task.Delay(0);

        }

        private void showApplicantApplicationProfile()
        {
            applicantApplicationProfileIsHidden = false;
            applicantJobPortalIsHidden = true;
            applicantMyJobsIsHidden = true;
        }
        private void showApplicantJobPortal()
        {
            applicantApplicationProfileIsHidden = true;
            applicantJobPortalIsHidden = false;
            applicantMyJobsIsHidden = true;
        }
        private void showApplicantMyJobs()
        {
            applicantApplicationProfileIsHidden = true;
            applicantJobPortalIsHidden = true;
            applicantMyJobsIsHidden = false;
        }

        private void showHRDataAnalyticsTool()
        {
            hrDataAnalyticsToolIsHidden = false;
            hrJobPortalIsHidden = true;
            hrCreateAJobIsHidden = true;
        }
        private void showHRJobPortal()
        {
            hrDataAnalyticsToolIsHidden = true;
            hrJobPortalIsHidden = false;
            hrCreateAJobIsHidden = true;
        }
        private void showHRCreateAJob()
        {
            hrDataAnalyticsToolIsHidden = true;
            hrJobPortalIsHidden = true;
            hrCreateAJobIsHidden = false;
        }

        private void Logout()
        {
            state.isLoggedIn = false;
        }

        private static string GetMultiSelectionTextLocation(List<string> selectedValues)
        {
            return $"Selected Location{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private static string GetMultiSelectionTextCompany(List<string> selectedValues)
        {
            return $"Selected Compan{(selectedValues.Count > 1 ? "ies" : "y")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private static string GetMultiSelectionTextDepartment(List<string> selectedValues)
        {
            return $"Selected Department{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private static string GetMultiSelectionTextJob(List<string> selectedValues)
        {
            return $"Selected Job Title{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private static string GetMultiSelectionTextType(List<string> selectedValues)
        {
            return $"Selected Type{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

    }

}
