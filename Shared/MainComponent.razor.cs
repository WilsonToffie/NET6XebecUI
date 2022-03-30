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
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage;

namespace XebecPortal.UI.Shared
{
    public partial class MainComponent
    {


        PersonalInformation personalInfo = new PersonalInformation();
        List<PersonalInformation> personalInfoList = new List<PersonalInformation>();
        private IList<Job> jobs = new List<Job>();
        private IList<JobType> jobTypes = new List<JobType>();

        private string Initials = "";
        private bool applicantApplicationProfile, applicantJobPortal, applicantMyJobs;

        private bool hrDataAnalyticsTool, hrJobPortal, hrCreateAJob;

        protected override async Task OnInitializedAsync()
        {
            jobs = await HttpClient.GetFromJsonAsync<IList<Job>>("https://xebecapi.azurewebsites.net/api/Job");

            jobTypes = await HttpClient.GetFromJsonAsync<IList<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");
            
            personalInfo = await HttpClient.GetFromJsonAsync<PersonalInformation>($"https://xebecapi.azurewebsites.net/api/personalinformation/{state.AppUserId}"); // !!!!!! Change the ID to be the userID later 
            Console.WriteLine("personalInfo: " + personalInfo.Id);
            Console.WriteLine("appUserID: " + state.AppUserId);
            if (state.Role.Equals("Candidate"))
            {
                showApplicantJobPortal();
            }
            else
            {
                showHRJobPortal();
            }
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


        private string storageAcc = "storageaccountxebecac6b";
        private string imgContainer = "images";
        private string userPicInfo;
        private async Task UploadingProfilePic(InputFileChangeEventArgs e)
        {
            // Getting the file
            var fileArray = e.File.Name.Split('.');
            userPicInfo = e.File.Name;
            var fileName = fileArray[0] + Guid.NewGuid().ToString().Substring(0, 5) + "." + fileArray[1];
            var fileInfo = e.File;
            // You require a azure account with a storage account. You use that link for below. The 'images' is the file that the file image is stored in in Azure.
            var blobUri = new Uri("https://"
                + storageAcc
                + ".blob.core.windows.net/"
                + imgContainer
                + "/"
                + fileName);

            AzureSasCredential credential = new AzureSasCredential("?sv=2020-08-04&ss=bfqt&srt=sco&sp=rwdlacupix&se=2024-12-02T20:36:17Z&st=2022-03-16T12:36:17Z&sip=1.1.1.1-255.255.255.255&spr=https&sig=nSCARiXySz%2BXLmtXJfZw28RkqfYUe%2FvDi11V9Q5Tpyo%3D");
            BlobClient blobClient = new BlobClient(blobUri, credential);

            var res = await blobClient.UploadAsync(fileInfo.OpenReadStream(1512000), new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = fileInfo.ContentType },
                TransferOptions = new StorageTransferOptions
                {
                    InitialTransferSize = 1024 * 1024,
                    MaximumConcurrency = 1
                }
            });

            if (res.GetRawResponse().Status <= 205)
            {
                
                // remember to fix / change later
                personalInfo.Id = state.AppUserId; // 1st person in DB THIS NEEDS TO CHANGE TO BE THEIR ID
                personalInfo.ImageUrl = blobUri.ToString();
                Console.WriteLine("Result is true whooooo");
                var content = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("url", $"{blobUri.ToString()}")
                                });
                //state.Avator = blobUri.ToString(); This displays whooooooooooooooooooo

                var resp = await HttpClient.PutAsJsonAsync($"https://xebecapi.azurewebsites.net/api/personalinformation/{personalInfo.Id}", personalInfo); //{personalInfo.Id}
            }
            else
            {
                Console.WriteLine("result is false :(");
            }
        }

        private void getInitials()
        {
            string firstInitial = state.Name.Substring(0, 1);
            string lastInitial = state.Surname.Substring(0, 1);
            Initials = firstInitial + lastInitial;
            state.Avator = Initials;
        }
    }
}
