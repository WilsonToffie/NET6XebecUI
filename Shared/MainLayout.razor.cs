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

        private string Initials = "";
        private string Avator = "";
        private bool applicantApplicationProfile, applicantJobPortal, applicantMyJobs = false;

        private bool hrDataAnalyticsTool, hrJobPortal, hrCreateAJob = false;

        protected override async Task OnInitializedAsync()
        {
            state.Role = "Candidate";
            state.isLoggedIn = true;
            applicantJobPortal = hrJobPortal = true;
            jobs = await HttpClient.GetFromJsonAsync<IList<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            jobTypes = await HttpClient.GetFromJsonAsync<IList<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");
        }

        private void showApplicantApplicationProfile()
        {
            applicantApplicationProfile = true;
            applicantJobPortal = false;
            applicantMyJobs = false;
        }
        private void showApplicantJobPortal()
        {
            applicantApplicationProfile = false;
            applicantJobPortal = true;
            applicantMyJobs = false;
        }
        private void showApplicantMyJobs()
        {
            applicantApplicationProfile = false;
            applicantJobPortal = false;
            applicantMyJobs = true;
        }

        private void showHRDataAnalyticsTool()
        {
            hrDataAnalyticsTool = true;
            hrJobPortal = false;
            hrCreateAJob = false;
        }
        private void showHRJobPortal()
        {
            hrDataAnalyticsTool = false;
            hrJobPortal = true;
            hrCreateAJob = false;
        }
        private void showHRCreateAJob()
        {
            hrDataAnalyticsTool = false;
            hrJobPortal = false;
            hrCreateAJob = true;
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
        private void getInitials()
        {
            Avator = state.Avator;
            string firstInitial = state.Name.Substring(0, 1);
            string lastInitial = state.Surname.Substring(0, 1);
            Initials = firstInitial + lastInitial;
        }

    }

}
