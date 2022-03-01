using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage;
using Azure;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XebecPortal.UI.Pages.Model;

namespace XebecPortal.UI.Pages.Applicant
{
    public partial class ApplicationProfile
    {
        private StringBuilder status = new StringBuilder("waiting");
        private ResumeResultModel resumeResultModel = new ResumeResultModel();
        
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
        
        private List<References> referencesList = new();
        private References references = new() { AppUserId = 1};
        

        private IJSObjectReference _jsModule;
        string _dragEnterStyle;
        IBrowserFile fileNames;
        private int maxAllowedSize = 10 * 1024 * 1024;
        private string progressBar = 0.ToString("0");

        protected override async Task OnInitializedAsync()
        {
            _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./jsPages/Applicant/ApplicationProfile.js");
        }
        
        private void AddReferences(References referencesValues)
        {
            
            referencesList.Add(new()
            {
                Id = increment,
                AppUserId = 1,
                Name = referencesValues.Name,
                Surname = referencesValues.Surname,
                Email = referencesValues.Email,
                ContactNum = referencesValues.ContactNum,
            });

            increment++;
        }

        private void DeleteReference(int refID)
        {
            referencesList.RemoveAll(x => x.Id == refID);
        }

        // Not entire sure how to set the data, still working on that
        private void SelectReference(int refID)
        {
           int val =  referencesList.FindIndex(x => x.Id == refID); // this refers to the List position to where it should be, just not sure how to set the input values
        }
        
        private async Task AddWorkHistory(WorkHistory workHistoryValues)
        {
            if (await _jsModule.InvokeAsync<bool>("WorkHistory"))
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
        }

        private void addWorkHistoryTest(WorkHistory workHistoryValues) {

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

        private async Task UpdateWorkHistory(WorkHistory workHistoryValues)
        {
            if (await _jsModule.InvokeAsync<bool>("WorkHistory"))
            {
                int index = workHistoryList.FindIndex(x => x.Id == workHistoryValues.Id);
                workHistoryList[index] = workHistoryValues;
                workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
                workHistUpdate = false;
            }
        }

        private void PopulateWorkHistory(int id)
        {
            workHistory = workHistoryList.FirstOrDefault(x => x.Id == id);
            workHistUpdate = true;
        }

        private async Task AddEducation(Education educationValues)
        {
            if (await _jsModule.InvokeAsync<bool>("Education"))
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
        }

        private void AddEducationTakeTwo(Education educationValues)
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

        private async Task UpdateEducation(Education educationValues)
        {
            if (await _jsModule.InvokeAsync<bool>("Education"))
            {
                int index = educationList.FindIndex(x => x.Id == educationValues.Id);
                educationList[index] = educationValues;
                education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
                eduUpdate = false;
            }
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
            if (await _jsModule.InvokeAsync<bool>("PersonalInformation"))
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

        private static int num = 1;
        async Task OnInputFileChangedAsync(InputFileChangeEventArgs e)
        {
            fileNames = e.File;
            progressBar = 0.ToString("0");
            status = new StringBuilder($"Uploading file {num++}");
            
            
            //Upload to blob - start
            status = new StringBuilder($"current file {fileNames.Name}");

            status.AppendLine("\n");
            
            var blobUri = new Uri("https://"
                                  + "amafilewam" +
                                  ".blob.core.windows.net/" +
                                  "upload" + "/" + fileNames.Name);
            AzureSasCredential credential = new AzureSasCredential(
                "sp=racwdli&st=2022-02-28T08:30:27Z&se=2022-03-11T16:30:27Z&sv=2020-08-04&sr=c&sig=TE%2B2VCz%2B6KKFbYHIkQwxGPOYWVUtht3xBPYZ8bE3kH4%3D");
            BlobClient blobClient = new BlobClient(blobUri, credential, new BlobClientOptions());
            status.AppendLine("Created blob client");
            
            status.AppendLine("\n");
            status.AppendLine("Sending to blob");
            //displayProgress = true;
            var res = await blobClient.UploadAsync(fileNames.OpenReadStream(maxAllowedSize), new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = fileNames.ContentType },
                TransferOptions = new StorageTransferOptions
                {
                    InitialTransferSize = 1024 * 1024,
                    MaximumConcurrency = 10
                },
                ProgressHandler = new Progress<long>((progress) =>
                {
                    progressBar = (100.0 * progress / fileNames.Size).ToString("0");
                })
            });

             if (Convert.ToInt32(progressBar) == 100)
            {
                //var content = new StringContent($"\"{blobUri.ToString()}\"",  Encoding.UTF8, "applicationModel/json");
                
                var content = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("url", $"{blobUri.ToString()}")
                                });
                // var urlJson =
                //     new StringContent(JsonSerializer.Serialize("content"""), Encoding.UTF8, "applicationModel/json");
                
                //var response = await httpClient.GetAsync("https://xebecapi.azurewebsites.net/api/ResumeParser");
                
                
                var resp =  await httpClient.PostAsync("http://localhost:5002/api/ResumeParser/", content);
                //status = new StringBuilder(resp.StatusCode.ToString());
                var respContent = await resp.Content.ReadAsStringAsync();

                resumeResultModel = JsonConvert.DeserializeObject<ResumeResultModel>(respContent);
                
                Console.WriteLine($"Content {respContent}");
                Console.WriteLine($"Result model {resumeResultModel}");
                //resumeResultModel =  System.Text.Json.JsonSerializer.Deserialize<ResumeResultModel>(respContent);

                
                    personalInformation.FirstName = resumeResultModel.Name;
                    personalInformation.Email = resumeResultModel.EmailAddress;

                    education.Insitution = resumeResultModel.CollegeName;
                    education.Qualification = resumeResultModel.CollegeName;

                    workHistory.CompanyName = resumeResultModel.CompaniesWorkedAt;
                    workHistory.JobTitle = resumeResultModel.Designation;
                


                //status = new StringBuilder(await resp.Content.ReadAsStringAsync());
            }

        }

        private void ResetFileNames()
        {
            fileNames = null;
        }

        void Upload()
        {
            //Upload the files here
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            Snackbar.Add("TODO: Upload your files!", Severity.Normal);
        }

    }
}