using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace XebecPortal.UI.Pages.Applicant
{
    public partial class ApplicationProfile
    {
        private int increment = 1;
        private bool workHistUpdate;
        private bool eduUpdate;
        private List<WorkHistory> workHistoryList = new();
        private WorkHistory workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        private List<Education> educationList = new();
        private Education education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        private ProfilePortfolioLink profilePortfolio = new() { AppUserId = 1 };
        private AdditionalInformation additionalInformation = new() { AppUserId = 1, Disability = "No" };
        private PersonalInformation personalInformation = new() { AppUserId = 1 };

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            jsRuntime.InvokeVoidAsync("Dropzone");
            return base.OnAfterRenderAsync(firstRender);
        }

        private void AddWorkHistory(WorkHistory workHistoryValues)
        {
            workHistoryList.Add(new()
            {
                Id = increment,
                AppUserId = 1,
                CompanyName = workHistoryValues.CompanyName,
                JobTitle = workHistoryValues.JobTitle,
                StartDate = workHistoryValues.StartDate,
                EndDate = workHistoryValues.EndDate,
                Description = workHistoryValues.Description
            });

            increment++;
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        }

        private void DeleteWorkHistory(int id)
        {
            workHistoryList.RemoveAll(x => x.Id == id);
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            workHistUpdate = false;
        }

        private void UpdateWorkHistory(WorkHistory workHistoryValues)
        {
            int index = workHistoryList.FindIndex(x => x.Id == workHistoryValues.Id);
            workHistoryList[index] = workHistoryValues;
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            workHistUpdate = false;
        }

        private void PopulateWorkHistory(int id)
        {
            workHistory = workHistoryList.FirstOrDefault(x => x.Id == id);
            workHistUpdate = true;
        }

        private void AddEducation(Education educationValues)
        {
            educationList.Add(new()
            {
                Id = increment,
                AppUserId = 1,
                Insitution = educationValues.Insitution,
                Qualification = educationValues.Qualification,
                StartDate = educationValues.StartDate,
                EndDate = educationValues.EndDate,
            });

            increment++;
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        }

        private void DeleteEducation(int id)
        {
            educationList.RemoveAll(x => x.Id == id);
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            eduUpdate = false;
        }

        private void UpdateEducation(Education educationValues)
        {
            int index = educationList.FindIndex(x => x.Id == educationValues.Id);
            educationList[index] = educationValues;
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            eduUpdate = false;
        }

        private void PopulateEducation(int id)
        {
            education = educationList.FirstOrDefault(x => x.Id == id);
            eduUpdate = true;
        }

        private void StartDateCheck()
        {
            workHistory.StartDate = workHistory.StartDate > workHistory.EndDate ? workHistory.EndDate : workHistory.StartDate;
            education.StartDate = education.StartDate > education.EndDate ? education.EndDate : education.StartDate;
        }

        private void EndDateCheck()
        {
            workHistory.EndDate = workHistory.EndDate < workHistory.StartDate ? workHistory.StartDate : workHistory.EndDate;
            education.EndDate = education.EndDate < education.StartDate ? education.StartDate : education.EndDate;
        }

        private async Task Submit()
        {
            if (await jsRuntime.InvokeAsync<bool>("ProfileApplication"))
            {
                foreach (var item in workHistoryList)
                    item.Id = 0;

                foreach (var item in educationList)
                    item.Id = 0;

                await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/PersonalInformation", personalInformation);
                await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/AdditionalInformation", additionalInformation);
                await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/WorkHistory/List", workHistoryList);
                await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/Education/List", educationList);
                await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/ProfilePortfolioLink", profilePortfolio);

                await jsRuntime.InvokeVoidAsync("alert", "You Data Has Been Captured");
            }


        }
    }
}