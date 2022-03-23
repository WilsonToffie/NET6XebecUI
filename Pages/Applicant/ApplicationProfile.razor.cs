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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;

namespace XebecPortal.UI.Pages.Applicant
{
    public partial class ApplicationProfile
    {
        private StringBuilder status = new StringBuilder("waiting");
        private ResumeResultModel resumeResultModel = new ResumeResultModel();

        private int increment = 1;
        private bool workHistUpdate;
        private bool eduUpdate;
        private bool editMode;
        private bool workEditMode;
        private bool eduEditMode;

        private string storageAcc = "storageaccountxebecac6b";
        private string imgContainer = "images";

        private List<WorkHistory> workHistoryList = new();
        private WorkHistory workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        private List<Education> educationList = new();
        private Education education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        private ProfilePortfolioLink profilePortfolio = new() { AppUserId = 1 };
        private AdditionalInformation additionalInformation = new() { AppUserId = 1, Disability = "No" };
        private PersonalInformation personalInformation = new() { AppUserId = 1 }; // Not sure if it even stores the information correctly
        private List<PersonalInformation> personalInformationList = new();
        private List<References> referencesList = new();
        private References references = new() { AppUserId = 1 };


        private List<string> skills = new List<string>(); // The skills that we current display
        private SkillsInformation skillInfo = new() { AppUserId = 1}; // the skill information model 
        private List<string> selectedSkillsList = new List<string>();

        private List<SkillsInformation> selectedSkillsList2 = new(); // the list that contains the information



        private IJSObjectReference _jsModule;
        string _dragEnterStyle;
        IBrowserFile fileNames;
        private int maxAllowedSize = 10 * 1024 * 1024;
        private string progressBar = 0.ToString("0");


        private bool educationProgressVal = false;
        private bool workProgressVal = false;
        private bool referenceProgressVal = false;

        ElementReference dropzone;
        InputFile file;
        private string thisFile;
        IJSObjectReference module, dropZone;


        //Create a skills list , with mock data just for now
        //Create a selected skill list
        //Then wriite that selected skill to the DB

        protected override async Task OnInitializedAsync()
        {
            
            _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./jsPages/Applicant/ApplicationProfile.js");
            populateSkillLists();

            try
            {
                module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./DragAndDrop.js");

                dropZone = await module.InvokeAsync<IJSObjectReference>("InitializeFileDropZone", dropzone, file);
            }
            catch
            {

            }
        }

        public class FileModel
        {
            public string FileName { get; set; }
        }

        FileModel newFile = new FileModel();

        [Parameter]
        public EventCallback<string> FileChanged { get; set; }


        public Task OnFileChange(InputFileChangeEventArgs e)
        {
            thisFile = e.File.Name.ToString();
            Console.WriteLine(thisFile+" in OnFileChange");
            return FileChanged.InvokeAsync(newFile.FileName);
        }

        public async ValueTask DisposeAsync()
        {
            if (dropZone != null)
            {
                await dropZone.InvokeVoidAsync("Dispose");

                await dropZone.DisposeAsync();
            }

            if (module != null)
            {
                await module.DisposeAsync();
            }
        }

        public void populateSkillLists()
        {
            skills.Add("Java");
            skills.Add("Python");
            skills.Add("C#");
            skills.Add("C");
            skills.Add("C++");
            skills.Add("mySQL");
            skills.Add("Blazor Framework");
            skills.Add("Azure services");
        }

        private void addToSelectedInfo(string info)
        {
            //if (!selectedSkillsList.Contains(info))
            //{
            //    selectedSkillsList.Add(info);
            //}
            //else
            //{
            //    // inform user that it existed already
            //}

            //  var test = selectedSkillsList2.FindAll(r => r.Description.Equals(info));

            selectedSkillsList2.Add(new()
               {
                   // Id = skillsId,// Is it auto increment
                    Description = info,
                    AppUserId = 1,
              });

            

        }
        private void removeFromSelectedInfo(SkillsInformation info)
        {
            selectedSkillsList2.RemoveAll(x => x.Description.Equals(info.Description)); ;
        }
        /* Use later
        private static string GetMultiSelectionTextSkills(List<string> selectedValues)
        {
            return $"Selected Skill{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }
        */
        /* wait for the DB  then I can use this
        private object CardClassSelect(Skills developer)
        {
            if (selectedSkills.FindAll(d => d.id == developer.id).Count() > 0)
                return "card-class-for-skills-selected";
            return "card-class-for-skills";
        }
        */
        private void AddReferences()
        {
            var validCheck = referencesList.FindAll(r => r.Name.Equals(references.Name) && string.Equals(r.Surname, references.Surname, StringComparison.OrdinalIgnoreCase) && string.Equals(r.Email, references.Email, StringComparison.OrdinalIgnoreCase) && string.Equals(r.ContactNum, references.ContactNum, StringComparison.OrdinalIgnoreCase));

            var emptyCheck = referencesList.FindAll(r => string.IsNullOrEmpty(r.Name) || string.IsNullOrEmpty(r.Surname) || string.IsNullOrEmpty(r.Email) || string.IsNullOrEmpty(r.ContactNum));

            referencesList.Add(new()
            {
                AppUserId = 1,
                Name = references.Name,
                Surname = references.Surname,
                Email = references.Email,
                ContactNum = references.ContactNum,
            });           
            references = new();
        } 

        private References tempRef;

        private void Save(References referenceValues)
        {
            editMode = false;
            Logger.LogInformation("Valid submit called");
            int index = referencesList.FindIndex(x => x.Equals(referenceValues));
            referencesList[index] = references;
            references = new();
        }

        private void Cancel(References referenceValues)
        {
            int index = referencesList.FindIndex(x => x.Equals(referenceValues));            
            referencesList[index] = tempRef;
            references = new();
            editMode = false;

        }

        private void DeleteReference(int refID)
        {            
            if (referencesList.Count == 1)
            {
                referenceProgressVal = true;
            }
            referencesList.RemoveAll(x => x.Id == refID);
        }


        private void SelectReference(References referenceValues)
        {
            editMode = true;
            int index = referencesList.FindIndex(x => x.Equals(referenceValues));
            references = referencesList[index];
            tempRef = (References)references.Clone();
        }
      
        private WorkHistory tempWorkHistory;

        private void addWorkHistoryTest()
        {
            workHistoryList.Add(new()
            {
                AppUserId = 1,
                CompanyName = workHistory.CompanyName,
                JobTitle = workHistory.JobTitle,
                StartDate = workHistory.StartDate,
                EndDate = workHistory.EndDate,
                Description = workHistory.Description
            });
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        }

        private void DeleteWorkHistory(WorkHistory workHistoryValues)
        {
            if (workHistoryList.Count == 1)
            {
                workProgressVal = true;
            }            
            workHistoryList.RemoveAll(x => x == (workHistoryValues));
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            workHistUpdate = false;
        }

        private void SelectWorkHistory(WorkHistory workHistoryValues)
        {            
            workEditMode = true;
            int index = workHistoryList.FindIndex(x => x == (workHistoryValues)); 
            workHistory = workHistoryList[index];
            tempWorkHistory = (WorkHistory)workHistory.Clone();           
        }

        // This is to display the selectedHistory tab
        private object GetStyling(WorkHistory item)
        {
            if ((workHistory.CompanyName == item.CompanyName) && (workHistory.JobTitle == item.JobTitle) && (workHistory.Description == item.Description))
                return "box-shadow: inset 0px -50px 36px -28px #49E5EF, inset 0px -50px 36px -28px #2294E3, inset 0px -50px 36px -28px #d35bc9, inset 0px -50px 36px -28px #00bcae;background: rgba(255, 255, 255, 0);backdrop - filter: blur(5.6px);-webkit - backdrop - filter: blur(5.6px);border: 1px solid rgba(255, 255, 255, 0.04);max - height: 60vh;overflow - y: auto; ";
            return "";
        }

        private object GetEduStyling(Education item)
        {
            if ((education.Insitution == item.Insitution) && (education.Qualification == item.Qualification))
                return "box-shadow: inset 0px -50px 36px -28px #49E5EF, inset 0px -50px 36px -28px #2294E3, inset 0px -50px 36px -28px #d35bc9, inset 0px -50px 36px -28px #00bcae;backdrop - filter: blur(5.6px);-webkit - backdrop - filter: blur(5.6px);border: 1px solid rgba(255, 255, 255, 0.04);max - height: 60vh;overflow - y: auto; ";
            return "";
        }
        private object GetRefStyling(References item)
        {
            if ((references.Email == item.Email) && (references.Name == item.Name) && (references.ContactNum == item.ContactNum) && (references.Email == item.Email))
                return "box-shadow: inset 0px -50px 36px -28px #49E5EF, inset 0px -50px 36px -28px #2294E3, inset 0px -50px 36px -28px #d35bc9, inset 0px -50px 36px -28px #00bcae;backdrop - filter: blur(5.6px);-webkit - backdrop - filter: blur(5.6px);border: 1px solid rgba(255, 255, 255, 0.04);max - height: 60vh;overflow - y: auto; ";
            return "";
        }
        private void SaveWorkHistory(WorkHistory workHistoryValues)
        {
            workEditMode = false;
            int index = workHistoryList.FindIndex(x => x.Equals(workHistoryValues));
            workHistoryList[index] = workHistory;
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        }

        private void CancelWorkHistory(WorkHistory workHistoryValues)
        {
            int index = workHistoryList.FindIndex(x => x.Equals(workHistoryValues));
            workHistoryList[index] = tempWorkHistory;
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            workEditMode = false;
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
        /*
        private void AddPersonallInformation()
        {           
            personalInformation.Add(new()
            {
                Id = increment,
                AppUserId = 1,
                Name = personalInformation.FirstName,
                Surname = references.Surname,
                Email = references.Email,
                ContactNum = references.ContactNum,
            });
            increment++;
            references = new();
        }

        */
        private Education tempEducation;
        private void AddEducationTakeTwo()
        {
            educationList.Add(new()
            {
                AppUserId = 1,
                Insitution = education.Insitution,
                Qualification = education.Qualification,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
            });         
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        }

        private void DeleteEducation(Education educationValues)
        {
            if (educationList.Count == 1)
            {
                educationProgressVal = true;
            }
            
            educationList.RemoveAll(x => x.Equals(educationValues));
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            eduUpdate = false;
        }

        private void SelectEducation(Education educationValues)
        {
            eduEditMode = true;
            int index = educationList.FindIndex(x => x.Equals(educationValues));
            education = educationList[index];
            tempEducation = (Education)education.Clone();
        }

        private void SaveEducation(Education educationValues)
        {
            eduEditMode = false;
            int index = educationList.FindIndex(x => x.Equals(educationValues));
            educationList[index] = education;
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        }

        private void CancelEducation(Education educationValues)
        {
            int index = educationList.FindIndex(x => x.Equals(educationValues));
            educationList[index] = tempEducation;
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            eduEditMode = false;
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
                await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/PersonalInformation", personalInformation);
                await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/AdditionalInformation", additionalInformation);

                foreach (var item in workHistoryList)
                {
                    await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/WorkHistory", item);
                }
                foreach (var item in educationList)
                {
                    await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/Education", item);
                }
                foreach (var item in referencesList)
                {
                await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/Reference", item);
                }

                foreach (var item in selectedSkillsList2)
                {
                   // await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/Education", item);
                }
             
                
                await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/ProfilePortfolioLink", profilePortfolio);

            if (await _jsModule.InvokeAsync<bool>("PersonalInformation"))
            {
                
            }
        }
        // This is just used to indicate to the user that their info has been successfully added to the DB
        /* using 
         (var msg = await httpClient.PostAsJsonAsync<LoginModel>("/api/auth/login", user,
         System.Threading.CancellationToken.None))
         {
            if (msg.IsSuccessStatusCode)
                 {
                    await jsRuntime.InvokeVoidAsync("alert", "You Data Has Been Captured");
                 }
             }
        */

        private static int num = 1;
        
        private async Task Upload()
        {
            Console.WriteLine(thisFile + " in Upload");
            progressBar = 0.ToString("0");

            var blobUri = new Uri("https://"
                + storageAcc
                + ".blob.core.windows.net/"
                + imgContainer
                + "/"
                + thisFile);
            Console.WriteLine(blobUri);
            AzureSasCredential credential = new AzureSasCredential("?sv=2020-08-04&ss=bfqt&srt=sco&sp=rwdlacupix&se=2024-12-02T20:36:17Z&st=2022-03-16T12:36:17Z&sip=1.1.1.1-255.255.255.255&spr=https&sig=nSCARiXySz%2BXLmtXJfZw28RkqfYUe%2FvDi11V9Q5Tpyo%3D");
            BlobClient blobClient = new BlobClient(blobUri, credential, new BlobClientOptions());
           
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
                    progressBar = (100.0 * progress / 1).ToString("0");
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


                var resp = await httpClient.PostAsync("http://localhost:44364/api/ResumeParser/", content);
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
            thisFile = "";
        }
    }
}