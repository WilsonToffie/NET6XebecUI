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
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using XebecPortal.UI.Utils.Handlers;

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
        private bool skillEditMode;
        private bool loadInfo;

        private List<WorkHistory> workHistoryList = new();
        private List<WorkHistory> addworkHistoryList = new();
        private WorkHistory workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        private List<Education> educationList = new();
        private List<Education> addEducationList = new();
        private Education education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        private ProfilePortfolioLink profilePortfolio = new();
        private List<ProfilePortfolioLink> profilePortfolioList = new();
        private AdditionalInformation additionalInformation = new();
        private PersonalInformation personalInformation = new(); // Not sure if it even stores the information correctly
        private List<PersonalInformation> personalInformationList = new();
        private List<AdditionalInformation> additionalInfoList = new();

        private References references = new();
        private List<References> referencesList = new();
        private List<References> addReferencesList = new();

        private List<SkillsInformation> selectedSkillsList1 = new();
        private List<SkillsInformation> addselectedSkillsList = new();
        SkillsInformation skillInfo = new SkillsInformation();

        private IList<SkillBank> apiSkills = new List<SkillBank>();
        private IList<SkillBank> skillListFilter = new List<SkillBank>();

        private IJSObjectReference _jsModule;
        string _dragEnterStyle;
        IBrowserFile fileNames;
        private int maxAllowedSize = 10 * 1024 * 1024;
        private string progressBar = 0.ToString("0");


        private bool educationProgressVal = false;
        private bool workProgressVal = false;
        private bool referenceProgressVal = false;
        private bool skillProgressVal = false;

        private APIRoot apiroot = new APIRoot();

        // This is used to place the information into the lists, if the user still exists
        private IList<WorkHistory> workHistories { get; set; }
        private IList<PersonalInformation> personalInfoHistory { get; set; }
        private IList<AdditionalInformation> additionalInfoHistory { get; set; }        
        private IList<Education> educationHistory { get; set; }
        private IList<ProfilePortfolioLink> profilePortfolioInfo { get; set; }
        private IList<References> referencesHistory{ get; set; }
        private IList<SkillsInformation> skillHistory { get; set; }

        private bool newPersonalInfo = false;
        private bool newAdditionalInfo = false;
        private bool newWorkHistoryInfo = false;
        private bool newEduInfo = false;
        private bool newSkillInfo = false;
        private bool newRefInfo = false;
        private bool newPortFolioInfo = false;

        // private CustomHandler cust = new CustomHandler();
        string token;
        protected override async Task OnInitializedAsync()
        {
            loadInfo = true;
            _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./jsPages/Applicant/ApplicationProfile.js");
            try
            {
                //var testToken = await localStorage.GetItemAsStringAsync("jwt_token");
                //var token = await localStorage.GetItemAsync<string>("jwt_token");                
                //Console.WriteLine("The working Token: " + token);
                //var response = await testJTW("https://xebecapi.azurewebsites.net/api/user/AllAuth",token);
                //var response = await cust.newwwwtestJTW("https://xebecapi.azurewebsites.net/api/user/AllAuth",token);
                //Console.WriteLine(response);
                //The following code allows the Lists to be populated with existing information
                token = await localStorage.GetItemAsync<string>("jwt_token");
                //Console.WriteLine("Token for personal info " + token);
               // var response = await httpClient.GetJsonAsync<string>("https://xebecapi.azurewebsites.net/api/user/AllAuth", new AuthenticationHeaderValue("Bearer", token));
                //Console.WriteLine("Does it work? " + response);


                personalInfoHistory = await httpClient.GetListJsonAsync<List<PersonalInformation>>($"https://xebecapi.azurewebsites.net/api/PersonalInformation", new AuthenticationHeaderValue("Bearer", token));//await httpClient.GetFromJsonAsync<List<PersonalInformation>>($"https://xebecapi.azurewebsites.net/api/PersonalInformation");
                personalInformationList = personalInfoHistory.Where(x => x.AppUserId == state.AppUserId).ToList();

                if (personalInformationList.Count == 0)
                {
                    newPersonalInfo = true;
                }
                else
                {
                    foreach (var item in personalInformationList)
                    {
                        personalInformation = item;
                    }
                }
                

                additionalInfoHistory = await httpClient.GetListJsonAsync<List<AdditionalInformation>>($"https://xebecapi.azurewebsites.net/api/AdditionalInformation", new AuthenticationHeaderValue("Bearer", token));
                additionalInfoList = additionalInfoHistory.Where(x => x.AppUserId == state.AppUserId).ToList();
                
                if (additionalInfoList.Count == 0)
                {
                    newAdditionalInfo = true;
                }
                else
                {
                    foreach (var item in additionalInfoList)
                    {                        
                        additionalInformation = item;
                    }
                }                

                workHistories = await httpClient.GetListJsonAsync<List<WorkHistory>>($"https://xebecapi.azurewebsites.net/api/WorkHistory", new AuthenticationHeaderValue("Bearer", token));
                workHistoryList = workHistories.Where(x => x.AppUserId == state.AppUserId).ToList();

                educationHistory = await httpClient.GetListJsonAsync<List<Education>>($"https://xebecapi.azurewebsites.net/api/Education", new AuthenticationHeaderValue("Bearer", token));
                educationList = educationHistory.Where(x => x.AppUserId == state.AppUserId).ToList();

                skillHistory = await httpClient.GetListJsonAsync<List<SkillsInformation>>($"https://xebecapi.azurewebsites.net/api/Skill", new AuthenticationHeaderValue("Bearer", token));
                selectedSkillsList1 = skillHistory.Where(x => x.AppUserId == state.AppUserId).ToList();

                referencesHistory = await httpClient.GetListJsonAsync<List<References>>($"https://xebecapi.azurewebsites.net/api/Reference", new AuthenticationHeaderValue("Bearer", token));
                referencesList = referencesHistory.Where(x => x.AppUserId == state.AppUserId).ToList();

                profilePortfolioInfo = await httpClient.GetListJsonAsync<List<ProfilePortfolioLink>>($"https://xebecapi.azurewebsites.net/api/ProfilePortfolioLink", new AuthenticationHeaderValue("Bearer", token));
                profilePortfolioList = profilePortfolioInfo.Where(x => x.AppUserId == state.AppUserId).ToList();

                if (profilePortfolioList.Count == 0)
                {
                    newPortFolioInfo = true;
                }
                else
                {
                    foreach (var item in profilePortfolioList)
                    {
                        profilePortfolio = item;
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error at pulling information from API " + e);
            }

            //selectedSkillsList1 = await httpClient.GetFromJsonAsync<List<SkillsInformation>>($"https://xebecapi.azurewebsites.net/api/WorkHistory/{state.AppUserId}");

            // Use this later to implement a massive variety of skills that is stored in the DB.
            //apiSkills = await httpClient.GetFromJsonAsync<IList<SkillBank>>("https://xebecapi.azurewebsites.net/api/SkillsBank");
            //populateList();
            //skillListFilter = apiSkills;

            // this is from a public API
            //var token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjNDNjZCRjIzMjBGNkY4RDQ2QzJERDhCMjI0MEVGMTFENTZEQkY3MUYiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJQR2FfSXlEMi1OUnNMZGl5SkE3eEhWYmI5eDgifQ.eyJuYmYiOjE2NDgyMTM1NTksImV4cCI6MTY0ODIxNzE1OSwiaXNzIjoiaHR0cHM6Ly9hdXRoLmVtc2ljbG91ZC5jb20iLCJhdWQiOlsiZW1zaV9vcGVuIiwiaHR0cHM6Ly9hdXRoLmVtc2ljbG91ZC5jb20vcmVzb3VyY2VzIl0sImNsaWVudF9pZCI6InF0dGF5Y2Y4cDdodWEwamIiLCJlbWFpbCI6ImFuZHJldy50cmF1dG1hbm5AMW5lYnVsYS5jb20iLCJjb21wYW55IjoiTmVidWxhIiwibmFtZSI6IkFuZHJldyBUcmF1dG1hbm4iLCJpYXQiOjE2NDgyMTM1NTksInNjb3BlIjpbImVtc2lfb3BlbiJdfQ.YYYk9_Jqlx6iCpySnxRTlSLSHKXg5MGB6qg5zTsO0Acc3SXdUcbJ3tBPCJHWLcVUQfWB3RusP6mpWavDBijOZyAoEZ8CVV9h7EfiToB4u1bd3CcnSOIU4-2vSsNOBCVp2HSzCP_SQwQYmBJkHVAerJqMnUSEpN_EOGcIdwDaVdM2ET5hXnm9wtUQzcnZ23x2cYeBFidOp2k5i6unMwuM6c5vcILQCTlYi2eXkZiDNwKNaamCxtHI4-NyJmGPL42D-efdMuw7b4tXnlkUn87sEWat0zpjcBK_ToUAwecD4ZuloBlDSToGnxo87MAh8hsN3wIxlKogCshF6NJaQb_sxw";
            // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //apiroot = await httpClient.GetFromJsonAsync<APIRoot>("https://emsiservices.com/skills/versions/latest/skills?limit=50");
            //var client = new RestClient("https://emsiservices.com/skills/versions/latest/skills"); // this will provide you with all of the available skills
            //var request = new RestRequest(Method.GET);
            //request.AddHeader("Authorization", "Bearer <ACCESS_TOKEN>");
            //IRestResponse response = client.Execute(request);

            progressCheck();
            completion();
            loadInfo = false;
        }

        //public async Task<string> testJTW(string apiEndpoint ,string Token)
        //{
        //    // Can get token here to prevent devs from getting the token on their page

        //    using (var request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint))
        //    {
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        //        var response = await httpClient.SendAsync(request);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine("It was successfull");
        //        }
        //       // response.EnsureSuccessStatusCode();

        //        return await response.Content.ReadAsStringAsync();
        //    }
        //}

        //private void loadUserInformation()
        //{

        //}

        private string skillWarning = "";
        private bool warning;


        //private void populateList()
        //{
        //    apiSkills.Add(new()
        //    {
        //        Description = "Java",
        //    });
        //    apiSkills.Add(new()
        //    {
        //        Description = "CSS",
        //    });
        //    apiSkills.Add(new()
        //    {
        //        Description = "C#",
        //    });
        //    apiSkills.Add(new()
        //    {
        //        Description = "Azure",
        //    });
        //}

        string searchedSkill;

        private async Task SearchSkillList(ChangeEventArgs e)
        {
            searchedSkill = e.Value.ToString(); // this value will be passed to the API to return the results
            Console.WriteLine("searchedSkill: " + searchedSkill);
            await Task.Delay(1500); // This delay will prevent the API call spam  when the the user types in the search values
            Console.WriteLine("text after delay: " + searchedSkill);
            skillListFilter = apiSkills; // apiSkill will be replaced with the new API link that will be called.

            if (string.IsNullOrEmpty(searchedSkill) && searchedSkill == " ")
            {
                // This will mainly be used to display most popular skills that users has chosen, still need to create a DB Table for that
                skillListFilter = skillListFilter.Where(x => x.Description.Contains(searchedSkill, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (searchedSkill?.Any() == true)
            {
                // This will display the requested search result.
                skillListFilter = skillListFilter.Where(x => x.Description.Contains(searchedSkill, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
        }

        //private void addToSelectedInfo(SkillBank info)
        //{
        //    warning = false;
        //    var validCheck = selectedSkillsList1.FindAll(r => r.Description.Equals(info.Description));
        //    if (validCheck.Count == 0)
        //    {
        //        selectedSkillsList1.Add(new()
        //        {
        //            Description = info.Description,
        //            AppUserId = 1,
        //        });
        //    }
        //    else
        //    {
        //        warning = true;
        //        skillWarning = "Skill has already been added!";
        //    }
        //}

        private async Task addToSelectedInfo()
        {
            selectedSkillsList1.Add(new()
            {
                Description = skillInfo.Description,
                AppUserId = state.AppUserId,
            });
            // This list is mainly used for POST requests as soon as the info is added, to ensure that there isn't duplicate info
            addselectedSkillsList.Add(new()
            {
                Description = skillInfo.Description,
                AppUserId = state.AppUserId,
            });

            foreach (var item in addselectedSkillsList)
            {                
                await httpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/Skill", item, new AuthenticationHeaderValue("Bearer", token));
                
            }
            addselectedSkillsList.Clear();// it immediately gets cleared after the POST.
            skillInfo = new();
            await OnInitializedAsync();         
        }


        private async Task removeFromSelectedInfo(SkillsInformation info)
        {            
            selectedSkillsList1.Remove(info);
            await httpClient.DeleteJsonAsync($"https://xebecapi.azurewebsites.net/api/Skill/{info.Id}", new AuthenticationHeaderValue("Bearer", token));
            skillInfo = new();
            await OnInitializedAsync();            
            skillEditMode = false;
            if (selectedSkillsList1.Count == 0)
            {
                skillProgressVal = true;
            }
        }
        private SkillsInformation skillTemp;
        private void SelectSkill(SkillsInformation value)
        {
            skillEditMode = true;
            int index = selectedSkillsList1.FindIndex(x => x.Equals(value));
            skillInfo = selectedSkillsList1[index];
            skillTemp = (SkillsInformation)skillInfo.Clone();
        }

        private async Task SaveSkill(SkillsInformation skillValue)
        {
            skillEditMode = false;
            int index = selectedSkillsList1.FindIndex(x => x.Equals(skillValue));
            selectedSkillsList1[index] = skillInfo;
            foreach (var item in selectedSkillsList1)
            {
                await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/Skill/{skillValue.Id}", item, new AuthenticationHeaderValue("Bearer", token));                
            }
            skillInfo = new();
            await OnInitializedAsync();
            
        }

        private void CancelSkill(SkillsInformation skillValue)
        {
            int index = selectedSkillsList1.FindIndex(x => x.Equals(skillValue));
            selectedSkillsList1[index] = skillTemp;
            skillInfo = new();
            skillEditMode = false;
        }

        private void AddPersonalInformation()
        {
            if (newPersonalInfo && newAdditionalInfo) // This is a check to see if the user is a "new" user to the system or an old user
            {
                personalInformationList.Add(new()
                {
                    FirstName = personalInformation.FirstName,
                    LastName = personalInformation.LastName,
                    PhoneNumber = personalInformation.PhoneNumber,
                    IdNumber = personalInformation.IdNumber,
                    Email = personalInformation.Email,
                    Address = personalInformation.Address,
                    AppUserId = state.AppUserId,
                });

                additionalInfoList.Add(new()
                {

                    Disability = "No",
                    Gender = additionalInformation.Gender,
                    Ethnicity = additionalInformation.Ethnicity,
                    AppUserId = state.AppUserId,
                });
            }
            else // If they are an already existing user, the information gets updated
            {
                personalInformationList.Add(new()
                {
                    Id = personalInformation.Id,
                    FirstName = personalInformation.FirstName,
                    LastName = personalInformation.LastName,
                    PhoneNumber = personalInformation.PhoneNumber,
                    IdNumber = personalInformation.IdNumber,
                    Email = personalInformation.Email,
                    Address = personalInformation.Address,
                    AppUserId = state.AppUserId,
                });

                additionalInfoList.Add(new()
                {
                    Id = additionalInformation.Id,
                    Disability = "No",
                    Gender = additionalInformation.Gender,
                    Ethnicity = additionalInformation.Ethnicity,
                    AppUserId = state.AppUserId,
                });
            }
            
        }

        private async Task AddReferences()
        {
            referencesList.Add(new()
            {                
                RefFirstName = references.RefFirstName,
                RefLastName = references.RefLastName,
                RefPhone = references.RefPhone,
                RefEmail = references.RefEmail,                
                AppUserId = state.AppUserId,
            });

            addReferencesList.Add(new()
            {
                RefFirstName = references.RefFirstName,
                RefLastName = references.RefLastName,
                RefPhone = references.RefPhone,
                RefEmail = references.RefEmail,
                AppUserId = state.AppUserId,
            });

            foreach (var item in addReferencesList)
            {
                await httpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/Reference", item, new AuthenticationHeaderValue("Bearer", token));
            }
            addReferencesList.Clear();
            await OnInitializedAsync();
            
            references = new();
        }

        private References tempRef;

        private async Task Save(References referenceValues)
        {
            editMode = false;
            int index = referencesList.FindIndex(x => x.Equals(referenceValues));
            referencesList[index] = references;
            foreach (var item in referencesList)
            {
                await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/Reference/{referenceValues.Id}", item, new AuthenticationHeaderValue("Bearer", token));                
            }
            references = new();
            await OnInitializedAsync();
            
        }

        private void Cancel(References referenceValues)
        {
            int index = referencesList.FindIndex(x => x.Equals(referenceValues));
            referencesList[index] = tempRef;
            references = new();
            editMode = false;
        }

        private async Task DeleteReference(References referenceValues)
        {
            referencesList.RemoveAll(x => x.Equals(referenceValues));
            await httpClient.DeleteJsonAsync($"https://xebecapi.azurewebsites.net/api/Reference/{referenceValues.Id}", new AuthenticationHeaderValue("Bearer", token));
            await OnInitializedAsync();
            references = new();
            editMode = false;
            if (referencesList.Count == 0)
            {
                referenceProgressVal = true;
            }
        }


        private void SelectReference(References referenceValues)
        {
            editMode = true;
            int index = referencesList.FindIndex(x => x.Equals(referenceValues));
            references = referencesList[index];
            tempRef = (References)references.Clone();
        }

        private WorkHistory tempWorkHistory;

        private async Task addWorkHistoryTest()
        {
            workHistoryList.Add(new()
            {
                AppUserId = state.AppUserId,
                CompanyName = workHistory.CompanyName,
                JobTitle = workHistory.JobTitle,
                StartDate = workHistory.StartDate,
                EndDate = workHistory.EndDate,
                Description = workHistory.Description
            });

            addworkHistoryList.Add(new()
            {
                AppUserId = state.AppUserId,
                CompanyName = workHistory.CompanyName,
                JobTitle = workHistory.JobTitle,
                StartDate = workHistory.StartDate,
                EndDate = workHistory.EndDate,
                Description = workHistory.Description
            });

            foreach (var item in addworkHistoryList)
            {
                await httpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/WorkHistory", item, new AuthenticationHeaderValue("Bearer", token));
            }
            await OnInitializedAsync();
            addworkHistoryList.Clear();
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        }

        private async Task DeleteWorkHistory(WorkHistory workHistoryValues)
        {            
            workHistoryList.RemoveAll(x => x == (workHistoryValues));
            await httpClient.DeleteJsonAsync($"https://xebecapi.azurewebsites.net/api/WorkHistory/{workHistoryValues.Id}", new AuthenticationHeaderValue("Bearer", token));
            await OnInitializedAsync();
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            workHistUpdate = false;
            workEditMode = false;
            if (workHistoryList.Count == 0)
            {
                workProgressVal = true;
            }
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
                return "background: #49E5EF;backdrop - filter: blur(5.6px);-webkit-backdrop-filter: blur(5.6px); border: 1px solid rgba(255, 255, 255, 0.04); min-height:15vh; overflow-y: auto;";
                //return "box-shadow: inset 0px -50px 36px -28px #49E5EF, inset 0px -50px 36px -28px #2294E3, inset 0px -50px 36px -28px #d35bc9, inset 0px -50px 36px -28px #00bcae;background: rgba(255, 255, 255, 0);backdrop - filter: blur(5.6px);-webkit-backdrop-filter: blur(5.6px); border: 1px solid rgba(255, 255, 255, 0.04); min-height:15vh; overflow-y: auto; ";
            return "";
        }
        //#49E5EF
        private object GetEduStyling(Education item)
        {
            if ((education.Insitution == item.Insitution) && (education.Qualification == item.Qualification))
                return "background: #49E5EF;backdrop - filter: blur(5.6px);-webkit-backdrop-filter: blur(5.6px); border: 1px solid rgba(255, 255, 255, 0.04); min-height:15vh; overflow-y: auto;";
            return "";
        }
        private object GetRefStyling(References item)
        {
            if ((references.RefFirstName == item.RefFirstName) && (references.RefLastName == item.RefLastName) && (references.RefPhone == item.RefPhone) && (references.RefEmail == item.RefEmail))
                return "background: #49E5EF;backdrop - filter: blur(5.6px);-webkit-backdrop-filter: blur(5.6px); border: 1px solid rgba(255, 255, 255, 0.04); min-height:15vh; overflow-y: auto;";
            return "";
        }
        
        //private object GetSkillStyling(SkillsInformation item)
        //{
        //    if (( == item.Description) && (references.Name == item.Name) && (references.ContactNum == item.ContactNum) && (references.Email == item.Email))
        //        return "background: #49E5EF;backdrop - filter: blur(5.6px);-webkit-backdrop-filter: blur(5.6px); border: 1px solid rgba(255, 255, 255, 0.04); min-height:15vh; overflow-y: auto;";
        //    return "";
        //}
        private async Task SaveWorkHistory(WorkHistory workHistoryValues)
        {
            workEditMode = false;
            int index = workHistoryList.FindIndex(x => x.Equals(workHistoryValues));
            workHistoryList[index] = workHistory;
            foreach (var item in workHistoryList)
            {
                await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/WorkHistory/{workHistoryValues.Id}", item, new AuthenticationHeaderValue("Bearer", token));
            }
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            await OnInitializedAsync();            
        }

        private void CancelWorkHistory(WorkHistory workHistoryValues)
        {
            int index = workHistoryList.FindIndex(x => x.Equals(workHistoryValues));
            workHistoryList[index] = tempWorkHistory;
            workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            workEditMode = false;
        }

        private Education tempEducation;
        private async Task AddEducationTakeTwo()
        {
            educationList.Add(new()
            {
                AppUserId = state.AppUserId,
                Insitution = education.Insitution,
                Qualification = education.Qualification,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
            });
            addEducationList.Add(new()
            {
                AppUserId = state.AppUserId,
                Insitution = education.Insitution,
                Qualification = education.Qualification,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
            });

            foreach (var item in addEducationList)
            {
                await httpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/Education", item, new AuthenticationHeaderValue("Bearer", token));
            }
            addEducationList.Clear();
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            await OnInitializedAsync();            
        }

        private async Task DeleteEducation(Education educationValues)
        {
            educationList.RemoveAll(x => x.Equals(educationValues));
            await httpClient.DeleteJsonAsync($"https://xebecapi.azurewebsites.net/api/Education/{educationValues.Id}", new AuthenticationHeaderValue("Bearer", token));
            await OnInitializedAsync();
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            eduUpdate = false;
            eduEditMode = false;

            if (educationList.Count == 0)
            {
                educationProgressVal = true;
            }
        }

        private void SelectEducation(Education educationValues)
        {
            eduEditMode = true;
            int index = educationList.FindIndex(x => x.Equals(educationValues));
            education = educationList[index];
            tempEducation = (Education)education.Clone();
        }

        private async Task SaveEducation(Education educationValues)
        {
            eduEditMode = false;
            int index = educationList.FindIndex(x => x.Equals(educationValues));
            educationList[index] = education;
            foreach (var item in educationList)
            {
                await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/Education/{educationValues.Id}", item, new AuthenticationHeaderValue("Bearer", token));
            }
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            await OnInitializedAsync();            
        }

        private void CancelEducation(Education educationValues)
        {
            int index = educationList.FindIndex(x => x.Equals(educationValues));
            educationList[index] = tempEducation;
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            eduEditMode = false;
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

        private void AddProfilePortfolio()
        {
            if (newPortFolioInfo)
            {
                profilePortfolioList.Add(new()
                {
                    GitHubLink = profilePortfolio.GitHubLink,
                    LinkedInLink = profilePortfolio.LinkedInLink,
                    TwitterLink = profilePortfolio.TwitterLink,
                    PersonalWebsiteUrl = profilePortfolio.PersonalWebsiteUrl,
                    AppUserId = state.AppUserId,
                });
            }
            else
            {
                profilePortfolioList.Add(new()
                {
                    Id = profilePortfolio.Id,
                    GitHubLink = profilePortfolio.GitHubLink,
                    LinkedInLink = profilePortfolio.LinkedInLink,
                    TwitterLink = profilePortfolio.TwitterLink,
                    PersonalWebsiteUrl = profilePortfolio.PersonalWebsiteUrl,
                    AppUserId = state.AppUserId,
                });
            }            
        }

        private async Task Submit()
        {
            AddProfilePortfolio();

            // test if this will work, otherwise a for each is required
            foreach (var item in personalInformationList)
            {
                if (newPersonalInfo)
                {
                    await httpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/PersonalInformation", item, new AuthenticationHeaderValue("Bearer", token));
                }
                else
                {
                    await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/PersonalInformation/{item.Id}", item, new AuthenticationHeaderValue("Bearer", token));
                }
                
            }
            foreach (var item in additionalInfoList)
            {
                Console.WriteLine("Additional Info List count " + additionalInfoList.Count);
                Console.WriteLine("Additional Info List ID's " + item.Id);
                if (newAdditionalInfo)
                {
                    await httpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/AdditionalInformation", item, new AuthenticationHeaderValue("Bearer", token));
                }
                else
                {
                    await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/AdditionalInformation/{item.Id}", item, new AuthenticationHeaderValue("Bearer", token));
                }

            }

            foreach (var item in profilePortfolioList)
            {                
                if (newPortFolioInfo)
                {
                    await httpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/ProfilePortfolioLink", item, new AuthenticationHeaderValue("Bearer", token));
                }
                else
                {
                    await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/ProfilePortfolioLink/{item.Id}", item, new AuthenticationHeaderValue("Bearer", token));
                }
            }

            //if (await _jsModule.InvokeAsync<bool>("PersonalInformation"))
            //{

            //}
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
        private string storageAcc = "storageaccountxebecac6b";
        private string imgContainer = "images";

        private static int num = 1;
        async Task OnInputFileChangedAsync(InputFileChangeEventArgs e)
        {
            fileNames = e.File;
            progressBar = 0.ToString("0");
            status = new StringBuilder($"Uploading file {num++}");


            //Upload to blob - start
            status = new StringBuilder($"current file {fileNames.Name}");

            status.AppendLine("\n");

            // Change the blobStorage location still
            var blobUri = new Uri("https://"
                                  + storageAcc
                                  + ".blob.core.windows.net/" 
                                  + imgContainer
                                  + "/"
                                  + fileNames.Name);
            Console.WriteLine("fileName " + fileNames.Name);

            AzureSasCredential credential = new AzureSasCredential("?sv=2020-08-04&ss=bfqt&srt=sco&sp=rwdlacupix&se=2024-12-02T20:36:17Z&st=2022-03-16T12:36:17Z&sip=1.1.1.1-255.255.255.255&spr=https&sig=nSCARiXySz%2BXLmtXJfZw28RkqfYUe%2FvDi11V9Q5Tpyo%3D");

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


                var resp = await httpClient.PostJsonAsync("http://localhost:5002/api/ResumeParser/", content, new AuthenticationHeaderValue("Bearer", token));
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
            /*Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            Snackbar.Add("TODO: Upload your files!", Severity.Normal);*/
        }

    }
}