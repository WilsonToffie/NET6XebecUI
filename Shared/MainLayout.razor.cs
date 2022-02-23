using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Shared.Home.Models;
using XebecPortal.UI.Pages.HR;

namespace XebecPortal.UI.Shared
{
    public partial class MainLayout
    {
        private MainModel mainmodel = new MainModel();
        private List<UserModel> users = new List<UserModel>();
        private UserModel thisUser = new UserModel();
        private IList<Job> jobs = new List<Job>();
        private IList<JobType> jobTypes = new List<JobType>();

        private bool HRIsHidden { get; set; } = true;
        private bool ApplicantIsHidden { get; set; } = true;

        private bool applicantApplicationProfileIsHidden { get; set; } = true;
        private bool applicantJobPortalIsHidden { get; set; } = true;
        private bool applicantMyJobsIsHidden { get; set; } = true;

        private bool hrApplicantPortalIsHidden { get; set; } = true;
        private bool hrDataAnalyticsToolIsHidden { get; set; } = true;
        private bool hrJobPortalIsHidden { get; set; } = true;
        private bool hrCreateAJobIsHidden { get; set; } = true;

        [Parameter]
        public int ID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            users = await HttpClient.GetFromJsonAsync<List<UserModel>>("https://my-json-server.typicode.com/IviweMalotana/xebecDB/Users");
            jobs = await HttpClient.GetFromJsonAsync<IList<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            jobTypes = await HttpClient.GetFromJsonAsync<IList<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");

            try
            {
                thisUser = users.Where(a => a.ID == ID).Single();

                if (thisUser.Role.Equals("applicant"))
                {
                    ApplicantIsHidden = false;

                    applicantApplicationProfileIsHidden = true;
                    applicantJobPortalIsHidden = true;
                    applicantMyJobsIsHidden = true;

                    HRIsHidden = true;

                }

                else if (thisUser.Role.Equals("hr"))
                {
                    HRIsHidden = false;
                    ApplicantIsHidden = true;

                    hrApplicantPortalIsHidden = true;
                    hrDataAnalyticsToolIsHidden = true;
                    hrJobPortalIsHidden = true;
                    hrCreateAJobIsHidden = false;
                }
            }
            catch
            {
                Console.WriteLine(mainmodel.ID + " not working");
            }

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
        private void showHRApplicantPortal()
        {
            hrApplicantPortalIsHidden = false;
            hrDataAnalyticsToolIsHidden = true;
            hrJobPortalIsHidden = true;
            hrCreateAJobIsHidden = true;
        }

        private void showHRDataAnalyticsTool()
        {
            hrApplicantPortalIsHidden = true;
            hrDataAnalyticsToolIsHidden = false;
            hrJobPortalIsHidden = true;
            hrCreateAJobIsHidden = true;
        }
        private void showHRJobPortal()
        {
            hrApplicantPortalIsHidden = true;
            hrDataAnalyticsToolIsHidden = true;
            hrJobPortalIsHidden = false;
            hrCreateAJobIsHidden = true;
        }
        private void showHRCreateAJob()
        {
            hrApplicantPortalIsHidden = true;
            hrDataAnalyticsToolIsHidden = true;
            hrJobPortalIsHidden = true;
            hrCreateAJobIsHidden = false;
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
