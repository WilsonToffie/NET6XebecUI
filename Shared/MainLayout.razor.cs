using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Shared.Home.Models;
using XebecPortal.UI.Pages.Applicant.Models;
using XebecPortal.UI.Pages.Applicant;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace XebecPortal.UI.Shared
{
    public partial class MainLayout
    {
        private IList<Job> jobs = new List<Job>();
        private IList<JobType> jobTypes = new List<JobType>();
        // Newly added stuffs
        /*
        private List<UploadResults> uploadResults = new();
        private List<Files> files = new();
        */
        private string Initials = "";
        private string Avator = "";
        private bool applicantApplicationProfile, applicantJobPortal, applicantMyJobs = false;

        private bool hrDataAnalyticsTool, hrJobPortal, hrCreateAJob = false;

        protected override async Task OnInitializedAsync()
        {
            state.isLoggedIn = true; ;
            state.Role = "Super Admin";
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


        /*
        private bool shouldRender;
        protected override bool ShouldRender() => shouldRender;


        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 15;
        private int maxAllowedFiles = 1;
        private bool isLoading;
        int count = 0;

        private async Task ImageFileUpload(InputFileChangeEventArgs e)
        {
            shouldRender = false;
            long maxFileSize = 1024 * 1024 * 15;
            var upload = false;

            using var content = new MultipartFormDataContent();

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                if (uploadResults.SingleOrDefault(
                    f => f.FileName == file.Name) is null)
                {
                    try
                    {
                        var fileContent =
                            new StreamContent(file.OpenReadStream(maxFileSize));

                        fileContent.Headers.ContentType =
                            new MediaTypeHeaderValue(file.ContentType);

                        files.Add(new() { Name = file.Name });

                        content.Add(
                            content: fileContent,
                            name: "\"files\"",
                            fileName: file.Name);

                        upload = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogInformation(
                            "{FileName} not uploaded (Err: 6): {Message}",
                            file.Name, ex.Message);

                        uploadResults.Add(
                            new()
                            {
                                FileName = file.Name,
                                ErrorCode = 6,
                                Uploaded = false
                            });
                    }
                }
            }

            if (upload)
            {
                // /Filesave this needs to change to the official
                var response = await HttpClient.PostAsJsonAsync("/Filesave", content);

                var newUploadResults = await response.Content
                    .ReadFromJsonAsync<IList<UploadResults>>();

                if (newUploadResults is not null)
                {
                    uploadResults = uploadResults.Concat(newUploadResults).ToList();
                }
            }

            shouldRender = true;
        }

        private static bool FileUpload(IList<UploadResults> uploadResults,  string? fileName, ILogger<MainLayout> logger, out UploadResults result)
        {
            result = uploadResults.SingleOrDefault(f => f.FileName == fileName) ?? new();

            if (!result.Uploaded)
            {
                logger.LogInformation("{FileName} not uploaded (Err: 5)", fileName);
                result.ErrorCode = 5;
            }

            return result.Uploaded;
        }

        */

        public void ImageFileUpload()
        {       
            
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
