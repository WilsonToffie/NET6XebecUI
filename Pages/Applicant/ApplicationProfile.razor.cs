using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Pages.Model;
using XebecPortal.UI.Utils.Handlers;

namespace XebecPortal.UI.Pages.Applicant
{
    public partial class ApplicationProfile
    {
        private StringBuilder status = new StringBuilder("waiting");
        private ResumeResultModel resumeResultModel = new ResumeResultModel();


        private bool savePersonalInfoPressed = false;
        private bool addWorkhistoryPressed = false;
        private bool addEducationPressed = false;
        private bool addMatricMarksPressed = false;
        private bool addSkillsPressed = false;
        private bool addReferencesPressed = false;
        private bool addLinksPressed = false;
        private bool fileAlreadyExists = false;

        private bool workHistUpdate;
        private bool eduUpdate;
        private bool editMode;
        private bool workEditMode;
        private bool eduEditMode;
        private bool skillEditMode;
        private bool updateSkillValue;
        private bool addSkillPage;
        private bool addWorkPage;
        private bool addEducationPage;
        private bool loadInfo;
        private bool profilePortfolioUpdate;
        private bool workHistoryDataExist;
        private bool educationHistoryDataExist;
        private bool skillDataExist;

        private bool addPersInfo;
        private bool updatedPersInfo;
        private bool addAdditionalInfo;
        private bool updateAdditionalInfo;

        private bool checkTestVal;

        private List<WorkHistory> workHistoryList = new List<WorkHistory>();
        private List<WorkHistory> addworkHistoryList = new();
        private WorkHistory selectedWorkHistory = new(); 
        private WorkHistory workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        

        private List<Education> educationList = new List<Education>();
        private List<Education> addEducationList = new();
        private Education education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
        private Education selectedEducation = new();
        
        private ProfilePortfolioLink profilePortfolio = new();
        private List<ProfilePortfolioLink> profilePortfolioList = new();
       
        private AdditionalInformation additionalInformation = new();
        private PersonalInformation personalInformation = new(); // Not sure if it even stores the information correctly
        private List<PersonalInformation> personalInformationList = new();
        private List<AdditionalInformation> additionalInfoList = new();
        

        private References references = new();
        private List<References> referencesList = new List<References>();
        private List<References> addReferencesList = new List<References>();

        private List<SkillsInformation> selectedSkillsList = new List<SkillsInformation>();
        private SkillsInformation selectedSkill = new();
        private List<SkillsInformation> addselectedSkillsList = new();
        SkillsInformation skillInfo = new SkillsInformation();

        private IList<SkillBank> apiSkills = new List<SkillBank>();
        private IList<SkillBank> skillListFilter = new List<SkillBank>();

        private List<Document> userDoc = new List<Document>();
        private List<Document> checkUserDoc = new List<Document>();
        private Document getUserDoc = new Document();

        private List<AdditionalDocument> additionalUserDocuments = new List<AdditionalDocument>();
        private AdditionalDocument additionalDocUploadInfo = new AdditionalDocument();

        private List<CVDocument> userCVDocuments= new List<CVDocument>();
        private CVDocument userCVDocUploadInfo = new CVDocument();
        private CVDocument getUserCVDoc = new CVDocument();
        //private CVDocument userCVDocUploadInfo = new CVDocument();

        private IJSObjectReference _jsModule;
        string _dragEnterStyle;
        IBrowserFile fileNames;
        private int maxAllowedSize = 1024 * 1024;
        private string progressBar = 0.ToString("0");


        private bool educationProgressVal = false;
        private bool workProgressVal = false;
        private bool referenceProgressVal = false;
        private bool skillProgressVal = false;

        // This is used to place the information into the lists, if the user still exists
        private IList<WorkHistory> workHistories { get; set; }
        private IList<PersonalInformation> personalInfoHistory { get; set; }
        private IList<AdditionalInformation> additionalInfoHistory { get; set; }        
        private IList<Education> educationHistory { get; set; }
        private IList<ProfilePortfolioLink> profilePortfolioInfo { get; set; }
        private IList<References> referencesHistory{ get; set; }
        private IList<SkillsInformation> skillHistory { get; set; }
        private IList<ProfilePicture> profilePicStuff { get; set; }
        private List<ProfilePicture> userProfilePicture = new List<ProfilePicture>();
        private IList<Document> userDocuments { get; set; }
        private IList<CVDocument> cvDocuments { get; set; }
        private IList<AdditionalDocument> additionalUserDocs { get; set; }
        private ProfilePicture profilePic = new ProfilePicture();

        private bool newPersonalInfo = false;
        private bool newAdditionalInfo = false;
        private bool newPortFolioInfo = false;
        private bool newDocumentInfo = false;
        private bool newAdditionalDocumentInfo = false;
        private bool validUpload = false;
        private bool updateWorkHistory = false;
        private bool updateEducation = false;
        private bool updateSkill = false;
        private bool updateReferences = false;
        private bool enterMatricMarks = false;
        private bool enterNewMark = false;
        private bool validMarkUpload = false;
        // This is used to prevent the users from uploading documents before the system has actually processed it
        private bool enableUploadButtons = true;
        private bool additionalDocuments = false;
        
        // private CustomHandler cust = new CustomHandler();
        string token;        
        private string userPic = String.Empty;
        private string updatedUserPic = String.Empty;
        private bool onlineProfileValidPost;
        private bool profilePictureExists;
        private bool editable;
        private bool showMagnifiedDocument;
        private bool cvDocumentExist;

        private int workHistoryId;
        private int eduHistoryId;
        private int existingSkillId;

        private Document doc = new Document();

        private List<matricMarks> matricInputs = new();
        private List<matricMarks> matricMarksAdded = new();
        private matricMarks marks = new();
        private string defaultCollaboratorImage = "/Img/DefaultImage.png";// "https://xebecstorage.blob.core.windows.net/profile-images/0";
        private string selectedDocument = String.Empty;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                loadInfo = true;

                editable = false;

            
                token = await localStorage.GetItemAsync<string>("jwt_token");
                Console.WriteLine("App user ID: " + state.AppUserId);
                await retrieveProfilePic();            
                await retrievePersonalAndAdditionalInfo();
                //_jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./jsPages/Applicant/ApplicationProfile.js");
                await retrieveWorkHistory();
            
                await retrieveEducationHistory();
            
                await retrieveSkills();

                await retrieveReferences();

                matricInputs = await httpClient.GetListJsonAsync<List<matricMarks>>($"MatricMark/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));

                await retrievecvDocuments();
                await retrieveAdditionalDocuments();

                await retrieveProfilePortfolio();           

                loadInfo = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not pull information from API, please try again: " + e);
            }

            //selectedSkillsList1 = await httpClient.GetFromJsonAsync<List<SkillsInformation>>($"WorkHistory/{state.AppUserId}");

            // Use this later to implement a massive variety of skills that is stored in the DB.
            //apiSkills = await httpClient.GetFromJsonAsync<IList<SkillBank>>("SkillsBank");
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

            //progressCheck();
            //completion();

        }

        // Reasons for these retrieval methods, is to improve response of the web page and to reduce slow downs, etc. Instead of calling the OnInitializedAsync() and it taking years to update, etc.
        private void changePersonalInfoEditableStatus(bool val)
        {
            if (editable)
            {
                editable = false;
            }
            else
            {
                editable = true;
            }            
        }
        private void changeSkillEditStatus(bool val)
        {
            if (skillEditMode)
            {
                skillEditMode = false;
            }
            else
            {
                skillEditMode = true;
            }
        }

        private async Task retrievePersonalAndAdditionalInfo()
        {
            personalInfoHistory = await httpClient.GetListJsonAsync<List<PersonalInformation>>($"PersonalInformation/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));//await httpClient.GetFromJsonAsync<List<PersonalInformation>>($"PersonalInformation");
            personalInformationList = personalInfoHistory.ToList();                                                                                                                                                                                                        //personalInfoHistory = await httpClient.GetListJsonAsync<List<PersonalInformation>>($"PersonalInformation", new AuthenticationHeaderValue("Bearer", token));//await httpClient.GetFromJsonAsync<List<PersonalInformation>>($"PersonalInformation");

            if (personalInformationList.Count == 0)
            {
                newPersonalInfo = true;
            }
            else
            {
                foreach (var item in personalInformationList)
                {
                    newPersonalInfo = false;
                    personalInformation = item;
                }
            }

            additionalInfoHistory = await httpClient.GetListJsonAsync<List<AdditionalInformation>>($"AdditionalInformation/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));
            additionalInfoList = additionalInfoHistory.ToList();

            // AdditionalInformation/{state.AppUserId}
            if (additionalInfoList.Count == 0)
            {
                newAdditionalInfo = true;
            }
            else
            {
                foreach (var item in additionalInfoList)
                {
                    newAdditionalInfo = false;
                    additionalInformation = item;
                }
            }
        }
        private async Task retrieveProfilePic()
        {
            profilePicStuff = await httpClient.GetListJsonAsync<List<ProfilePicture>>($"ProfilePicture", new AuthenticationHeaderValue("Bearer", token));
            userProfilePicture = profilePicStuff.Where(x => x.AppUserId == state.AppUserId).ToList();

            if (userProfilePicture.Count > 0)
            {
                profilePictureExists = true;
                foreach (var item in userProfilePicture)
                {
                    profilePic = item;
                    userPic = item.profilePic;
                    updatedUserPic = DateTime.Now.ToString();
                }
            }
            else
            {
                profilePictureExists = false;
                userPic = defaultCollaboratorImage;
            }
        }
        private async Task retrieveWorkHistory()
        {
            workHistoryList = await httpClient.GetListJsonAsync<List<WorkHistory>>($"WorkHistory/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));

            if (workHistoryList.Count == 0)
            {
                workHistoryDataExist = false;
            }
            else
            {
                workHistoryDataExist = true;
            }
            //workHistoryList = workHistories.Where(x => x.AppUserId == state.AppUserId).ToList();
        }

        private async Task retrieveEducationHistory()
        {
            educationList = await httpClient.GetListJsonAsync<List<Education>>($"Education/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));
            //educationList = educationHistory.Where(x => x.AppUserId == state.AppUserId).ToList();

            if (educationList.Count == 0)
            {
                educationHistoryDataExist = false;

            }
            else
            {
                educationHistoryDataExist = true;
            }
        }

        private async Task retrieveSkills()
        {
            selectedSkillsList = await httpClient.GetListJsonAsync<List<SkillsInformation>>($"Skill/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));
            // selectedSkillsList1 = skillHistory.ToList();

            if (selectedSkillsList.Count == 0)
            {
                skillDataExist = false;
            }
            else
            {
                skillDataExist = true;
            }
        }

        private async Task retrieveReferences()
        {
            referencesList = await httpClient.GetListJsonAsync<List<References>>($"Reference/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));
            //referencesList = referencesHistory.Where(x => x.AppUserId == state.AppUserId).ToList();
        }
        private async Task retrievecvDocuments()
        {
            cvDocuments = await httpClient.GetListJsonAsync<List<CVDocument>>($"CVDocument/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));
            userCVDocuments = cvDocuments.ToList();

            if (userCVDocuments.Count == 0)
            {
                cvDocumentExist = false;
            }
            else
            {
                cvDocumentExist = true;
                foreach (var item in userCVDocuments)
                {
                    getUserCVDoc = item;                   
                }
            }
            //userDocuments = await httpClient.GetListJsonAsync<List<Document>>($"Document/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));
            //checkUserDoc = userDocuments.ToList();
            //if (checkUserDoc.Count == 0)
            //{
            //    newDocumentInfo = true;
            //    cvDocumentExist = true;
            //}
            //else
            //{
            //    Console.WriteLine("user already has documents");
            //    foreach (var item in checkUserDoc)
            //    {
            //        getUserDoc = item;
            //        Console.WriteLine("RetrieveDoc Method CV val: " + getUserDoc.CV);
            //        if (string.IsNullOrEmpty(getUserDoc.CV))
            //        {
            //            cvDocumentExist = true;
            //        }
            //        else
            //        {
            //            cvDocumentExist = false;
            //        }

            //        if (string.IsNullOrEmpty(@item.MatricCertificate) && string.IsNullOrEmpty(@item.UniversityTranscript) && string.IsNullOrEmpty(@item.AdditionalCert1) && string.IsNullOrEmpty(@item.AdditionalCert2) && string.IsNullOrEmpty(@item.AdditionalCert3))
            //        {
            //            additionalDocuments = true;
            //        }
            //        else
            //        {
            //            additionalDocuments = false;
            //        }

            //    }
            //}
        }

        private async Task retrieveAdditionalDocuments()
        {
            additionalUserDocs = await httpClient.GetListJsonAsync<List<AdditionalDocument>>($"AdditionalDocument/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));
            additionalUserDocuments = additionalUserDocs.ToList();
            if (additionalUserDocuments.Count == 0)
            {
                newAdditionalDocumentInfo = true;
                //cvDocumentExist = true;
            }
            else
            {
                newAdditionalDocumentInfo = false;
                Console.WriteLine("user already has additional documents");
            }
        }

        private async Task retrieveProfilePortfolio()
        {
            profilePortfolioList = await httpClient.GetListJsonAsync<List<ProfilePortfolioLink>>($"ProfilePortfolioLink/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));
            //profilePortfolioList = profilePortfolioInfo.ToList();

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
        private string skillWarning = "";
        private bool warning;

        string searchedSkill;

        //private async Task SearchSkillList(ChangeEventArgs e)
        //{
        //    searchedSkill = e.Value.ToString(); // this value will be passed to the API to return the results
        //    Console.WriteLine("searchedSkill: " + searchedSkill);
        //    await Task.Delay(1500); // This delay will prevent the API call spam  when the the user types in the search values
        //    Console.WriteLine("text after delay: " + searchedSkill);
        //    skillListFilter = apiSkills; // apiSkill will be replaced with the new API link that will be called.

        //    if (string.IsNullOrEmpty(searchedSkill) && searchedSkill == " ")
        //    {
        //        // This will mainly be used to display most popular skills that users has chosen, still need to create a DB Table for that
        //        skillListFilter = skillListFilter.Where(x => x.Description.Contains(searchedSkill, StringComparison.CurrentCultureIgnoreCase)).ToList();
        //    }

        //    if (searchedSkill?.Any() == true)
        //    {
        //        // This will display the requested search result.
        //        skillListFilter = skillListFilter.Where(x => x.Description.Contains(searchedSkill, StringComparison.CurrentCultureIgnoreCase)).ToList();
        //    }
        //}

        //private SkillsInformation skillTemp;
        //private void SelectSkill(SkillsInformation value)
        //{
        //    skillEditMode = true;
        //    int index = selectedSkillsList1.FindIndex(x => x.Equals(value));
        //    skillInfo = selectedSkillsList1[index];
        //    skillTemp = (SkillsInformation)skillInfo.Clone();
        //}

        private async Task editSkill(int skillId , bool val)
        {
            updateSkillValue = val;
            existingSkillId = skillId;
            if (updateSkillValue && (skillId > 0))
            {
                foreach (var item in selectedSkillsList)
                {
                    if (item.Id == skillId)
                    {
                        selectedSkill.Id = item.Id;
                        selectedSkill.Description = item.Description;
                        selectedSkill.AppUserId = item.AppUserId;                        
                    }
                }
            }
        } 
        private void addSkillInfoPage( bool val)
        {
            addSkillPage = val;            
        }
        private void addWorkHistoryPage( bool val)
        {
            addWorkPage = val;            
        }
        private void addEducationHistoryPage( bool val)
        {
            addEducationPage = val;            
        }

        private async Task addSkill()
        {
            skillInfo.AppUserId = state.AppUserId;            
            var addSkillInfo = await httpClient.PostJsonAsync($"Skill", skillInfo, new AuthenticationHeaderValue("Bearer", token));
            if (addSkillInfo.IsSuccessStatusCode)
            {
                await jsRuntime.InvokeAsync<object>("alert", "Skill information has successfully been added!");
                skillInfo = new();
            }
            await retrieveSkills();
        }
        private async Task updateSkillInfo()
        {
            //skillEditMode = false;
            //int index = selectedSkillsList.FindIndex(x => x.Equals(skillValue));
            //selectedSkillsList[index] = skillInfo;
            //if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Override This Item?"))
            //{
            //    foreach (var item in selectedSkillsList)
            //    {
            //        var skillUpdate =  await httpClient.PutJsonAsync($"Skill/{skillValue.Id}", item, new AuthenticationHeaderValue("Bearer", token));
            //        if (skillUpdate.IsSuccessStatusCode)
            //        {
            //            updateSkill = true;

            //        } 
            //    }
            //    if (updateSkill)
            //    {
            //        await jsRuntime.InvokeAsync<object>("alert", "Skill information has successfully been changed!");
            //        updateSkill = false;
            //    }
            //}
            //skillInfo = new();
            //await retrieveSkills();
            //
            if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Override This Item?"))
            {
                foreach (var item in selectedSkillsList)
                {
                    var skillUpdate = await httpClient.PutJsonAsync($"Skill/{selectedSkill.Id}", item, new AuthenticationHeaderValue("Bearer", token));
                    if (skillUpdate.IsSuccessStatusCode)
                    {
                        updateSkill = true;

                    }
                }
                if (updateSkill)
                {
                    await jsRuntime.InvokeAsync<object>("alert", "Skill information has successfully been changed!");
                    updateSkill = false;
                }
            }
        }
        private async Task deleteSkill(int skillId)
        {
            selectedSkillsList.RemoveAll(x => x.Id == skillId);

            if (selectedSkillsList.Count == 0)
            {
                skillDataExist = false;
            }
            await httpClient.DeleteJsonAsync($"Skill/{skillId}", new AuthenticationHeaderValue("Bearer", token));            
          //  await retrieveSkills();            
        }

        // This needs to be tested.. Since it isn't necessary to add the values to a list and post it
        private async Task AddPersonalInformation()
        {
            
            if (newPersonalInfo)
            {
                personalInformation.AppUserId = state.AppUserId;
                var addedPersonalInfo = await httpClient.PostJsonAsync($"PersonalInformation", personalInformation, new AuthenticationHeaderValue("Bearer", token));

                if (addedPersonalInfo.IsSuccessStatusCode)
                {
                    addPersInfo = true;
                }
                else
                {
                    addPersInfo = false;
                }
            }
            else
            {
                var updatedPersonalInfo = await httpClient.PutJsonAsync($"PersonalInformation/{personalInformation.Id}", personalInformation, new AuthenticationHeaderValue("Bearer", token));

                if (updatedPersonalInfo.IsSuccessStatusCode)
                {
                    addPersInfo = true;
                }
                else
                {
                    addPersInfo = false;
                }
            }

            if (newAdditionalInfo)
            {
                additionalInformation.AppUserId = state.AppUserId;

                var addedAdditionalInfo = await httpClient.PostJsonAsync($"AdditionalInformation", additionalInformation, new AuthenticationHeaderValue("Bearer", token));
                if (addedAdditionalInfo.IsSuccessStatusCode)
                {
                    addAdditionalInfo = true;
                }
                else
                {
                    addAdditionalInfo = false;
                }
            }
            else
            {
                var updatedAdditionalInfo = await httpClient.PutJsonAsync($"AdditionalInformation/{additionalInformation.Id}", additionalInformation, new AuthenticationHeaderValue("Bearer", token));
                if (updatedAdditionalInfo.IsSuccessStatusCode)
                {
                    addAdditionalInfo = true;
                }
                else
                {
                    addAdditionalInfo = false;
                }
            }

            if (addPersInfo && addAdditionalInfo)
            {
                await jsRuntime.InvokeAsync<object>("alert", "Your information has been saved!");
                addPersInfo = false;                
                addAdditionalInfo = false;                
                await retrievePersonalAndAdditionalInfo();
            }
        }


        private async Task AddReferences()
        {
            addReferencesPressed = true;

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
                await httpClient.PostJsonAsync($"Reference", item, new AuthenticationHeaderValue("Bearer", token));
            }
            addReferencesList.Clear();
            await retrieveReferences();
            
            references = new();

            addReferencesPressed = false;
        }

        private References tempRef;

        private async Task Save(References referenceValues)
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Override This Item?"))
            {
                editMode = false;
                int index = referencesList.FindIndex(x => x.Equals(referenceValues));
                referencesList[index] = references;
                foreach (var item in referencesList)
                {
                    var success =  await httpClient.PutJsonAsync($"Reference/{referenceValues.Id}", item, new AuthenticationHeaderValue("Bearer", token));
                    if (success.IsSuccessStatusCode)
                    {
                        updateReferences = true;
                        
                    }
                }
                if (updateReferences)
                {
                    await jsRuntime.InvokeAsync<object>("alert", "Reference information has successfully been changed!");
                    updateReferences = false;
                }
            }
            references = new();
            await retrieveReferences();            
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
            await httpClient.DeleteJsonAsync($"Reference/{referenceValues.Id}", new AuthenticationHeaderValue("Bearer", token));
            await retrieveReferences();
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

        private async Task addWorkHistory()
        {            
            try
            {
                workHistory.AppUserId = state.AppUserId;
                var addWorkHistory = await httpClient.PostJsonAsync($"WorkHistory", workHistory, new AuthenticationHeaderValue("Bearer", token));

                if (addWorkHistory.IsSuccessStatusCode)
                {
                    await jsRuntime.InvokeAsync<object>("alert", "Work history has successfully been added");
                    workHistory = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
                    await retrieveWorkHistory();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error at adding job to the db: " + e);
            }
                    
        }
        private async Task DeleteWorkHistory(WorkHistory workHistoryValues)
        {            
            workHistoryList.RemoveAll(x => x == (workHistoryValues));
            if (workHistoryList.Count == 0)
            {
                workHistoryDataExist = false;
            }
            await httpClient.DeleteJsonAsync($"WorkHistory/{workHistoryValues.Id}", new AuthenticationHeaderValue("Bearer", token));            
        }

        private void editWorkHistoryStatus(int workId, bool val)
        {
            workHistUpdate = val;
            workHistoryId = workId;
            if (workHistUpdate && (workId > 0))
            {
                foreach (var item in workHistoryList)
                {
                    if (item.Id == workHistoryId)
                    {
                        selectedWorkHistory.Id = item.Id;
                        selectedWorkHistory.CompanyName = item.CompanyName;
                        selectedWorkHistory.JobTitle = item.JobTitle;
                        selectedWorkHistory.Description = item.Description;
                        selectedWorkHistory.StartDate = item.StartDate;
                        selectedWorkHistory.EndDate = item.EndDate;
                        selectedWorkHistory.AppUser = item.AppUser;
                        selectedWorkHistory.AppUserId = item.AppUserId;

                    }
                }
            }           
        }
        private async Task UpdateWorkHistory()
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm","Are you certain that you want to override existing information?"))
            {
                var workHistoryState = await httpClient.PutJsonAsync($"WorkHistory/{selectedWorkHistory.Id}", selectedWorkHistory, new AuthenticationHeaderValue("Bearer", token));
                if (workHistoryState.IsSuccessStatusCode)
                {
                    updateWorkHistory = true;
                }
                else
                {
                    updateWorkHistory = false; 
                }
                if (updateWorkHistory)
                {
                    await jsRuntime.InvokeAsync<object>("alert", "Work History information has successfully been changed!");
                    updateWorkHistory = false;
                }
            }
            await retrieveWorkHistory();            
        }       

        private async Task addEducation()
        {
            try
            {
                education.AppUserId = state.AppUserId;
                var addEducation = await httpClient.PostJsonAsync($"Education", education, new AuthenticationHeaderValue("Bearer", token));

                if (addEducation.IsSuccessStatusCode)
                {
                    await jsRuntime.InvokeAsync<object>("alert", "Education has successfully been added");
                    education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
                    await retrieveEducationHistory();
                }                
            }
            catch(Exception e)
            {
                Console.WriteLine("Error at adding education to the db: " + e);
            }            
        }
        private async Task DeleteEducation(Education educationValues)
        {
            try
            {
                educationList.RemoveAll(x => x == (educationValues));
                if (educationList.Count == 0)
                {
                    educationHistoryDataExist = false;
                }
                await httpClient.DeleteJsonAsync($"Education/{educationValues.Id}", new AuthenticationHeaderValue("Bearer", token));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error at deleting education from the db: " + e);
            }
            
            //await retrieveEducationHistory(); you dont have to recall it, since it isn't necessary
        }
        private async Task UpdateEducation()
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Override This Item?"))
            {                
                var educationState = await httpClient.PutJsonAsync($"Education/{selectedEducation.Id}", selectedEducation, new AuthenticationHeaderValue("Bearer", token));
                if (educationState.IsSuccessStatusCode)
                {
                    updateEducation = true;
                }

                if (updateEducation)
                {
                    await jsRuntime.InvokeAsync<object>("alert", "Education information has successfully been changed!");
                    updateEducation = false;
                }
            }
            education = new() { StartDate = DateTime.Today, EndDate = DateTime.Today };
            await retrieveEducationHistory();            
        }
        private void editEducationStatus(int eduId, bool val)
        {
            eduUpdate = val;
            eduHistoryId = eduId;
            if (eduUpdate && (eduId > 0))
            {
                foreach (var item in educationList)
                {
                    if (item.Id == eduHistoryId)
                    {
                        selectedEducation.Id = item.Id;
                        selectedEducation.Insitution = item.Insitution;
                        selectedEducation.Qualification = item.Qualification;
                        selectedEducation.StartDate = item.StartDate;
                        selectedEducation.EndDate = item.EndDate;
                        selectedEducation.AppUserId = item.AppUserId;
                        selectedEducation.AppUser = item.AppUser;
                    }
                }
            }
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

        private async Task AddProfileLinks()
        {
            Console.WriteLine("The button should be disabled");
            addLinksPressed = true;
            //StateHasChanged();
            // test if this will work, otherwise a for each is required            
            AddProfilePortfolio();
            foreach (var item in profilePortfolioList)
            {                
                if (newPortFolioInfo)
                {
                    var validPost = await httpClient.PostJsonAsync($"ProfilePortfolioLink", item, new AuthenticationHeaderValue("Bearer", token));
                    if (validPost.IsSuccessStatusCode)
                    {
                        onlineProfileValidPost = true;
                    }
                    else
                    {
                        onlineProfileValidPost = false;
                    }
                }
                else
                {
                    var validPost = await httpClient.PutJsonAsync($"ProfilePortfolioLink/{item.Id}", item, new AuthenticationHeaderValue("Bearer", token));
                    if (validPost.IsSuccessStatusCode)
                    {
                        onlineProfileValidPost = true;
                    }
                    else
                    {
                        onlineProfileValidPost = false;
                    }
                }
            }

            if (onlineProfileValidPost)
            {
                await jsRuntime.InvokeAsync<object>("alert", "Portfolio information has been saved!");
            }

            addLinksPressed = false;
            //StateHasChanged();
        }

        private void profilePortfolioEditStatus( bool val)
        {
            
            if (profilePortfolioUpdate)
            {
                profilePortfolioUpdate = false;
            }
            else
            {
                profilePortfolioUpdate = true;
            }
        }



        private string storageAcc = "xebecstorage";
        private string imgContainer = "linkedincv";
        private string azureCredentials = "?sv=2020-08-04&ss=bfqt&srt=sco&sp=rwdlacupx&se=2022-09-11T22:04:48Z&st=2022-05-11T14:04:48Z&spr=https&sig=MTkK0ODHx%2Fj%2BBwTPXnhcauQN%2F8A1HPhfG0kA%2BGKklmE%3D";
        private static int num = 1;


        async Task OnInputFileChangedAsync(InputFileChangeEventArgs e)
        {
            enableUploadButtons = false;

            //https://xebecstorage.blob.core.windows.net/linkedincv
            fileNames = e.File;
            progressBar = 0.ToString("0");
            status = new StringBuilder($"Uploading file {num++}");
            var fileName = state.AppUserId;


            if (e.File.Size > maxAllowedSize)
            {
                await jsRuntime.InvokeAsync<object>("alert", "The document size is too large, please upload a document that is < 1mb");
            }
            else
            {
                //Upload to blob - start
                status = new StringBuilder($"current file {fileNames.Name}");

                status.AppendLine("\n");

                // Change the blobStorage location still
                var blobUri = new Uri("https://"
                                      + storageAcc
                                      + ".blob.core.windows.net/" 
                                      + imgContainer
                                      + "/"
                                      + fileName);
                Console.WriteLine("fileName " + fileNames.Name);

                AzureSasCredential credential = new AzureSasCredential(azureCredentials);

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

                    //var response = await httpClient.GetAsync("ResumeParser");
                    userCVDocUploadInfo.documentBlobLink = blobUri.ToString();
                    userCVDocUploadInfo.documentName = e.File.Name;
                    userCVDocUploadInfo.AppUserId = state.AppUserId;               
                    if (!cvDocumentExist)
                    {
                        var resp = await httpClient.PostJsonAsync($"CVDocument", userCVDocUploadInfo, new AuthenticationHeaderValue("Bearer", token));
                        if (resp.IsSuccessStatusCode)
                        {
                            validUpload = true;                                      
                        }
                    }
                    else
                    {
                        userCVDocUploadInfo.Id = getUserCVDoc.Id;
                        userCVDocUploadInfo.AppUserId = userCVDocUploadInfo.AppUserId;
                        var resp = await httpClient.PutJsonAsync($"CVDocument/{getUserCVDoc.Id}", userCVDocUploadInfo, new AuthenticationHeaderValue("Bearer", token));

                        if (resp.IsSuccessStatusCode)
                        {
                            userDoc.Clear();
                            validUpload = true;
                        }
                    }
                    if (validUpload)
                    {
                        //await OnInitializedAsync();
                        await jsRuntime.InvokeAsync<object>("alert", "Your CV has successfully been uploaded");
                        userCVDocUploadInfo = new();
                        await retrievecvDocuments();
                    }                
                }                
            }
            enableUploadButtons = true;
        }
        // This is used to delete the CV from the Blob Storage

        async Task DeleteCV(CVDocument e)
        {
            var fileName = state.AppUserId;
            var blobUri = new Uri("https://"
                                  + storageAcc
                                  + ".blob.core.windows.net/"
                                  + imgContainer
                                  + "/"
                                  + fileName);
           // Console.WriteLine("fileName " + fileNames.Name);

            AzureSasCredential credential = new AzureSasCredential(azureCredentials);

            BlobClient blobClient = new BlobClient(blobUri, credential, new BlobClientOptions());
            status.AppendLine("Created blob client");

            status.AppendLine("\n");
            status.AppendLine("Sending to blob");
            //displayProgress = true;
            var res = await blobClient.DeleteIfExistsAsync();            

            if (res.GetRawResponse().Status <= 205)
            {
                Console.WriteLine("Successfully deleted file from the blob");
                var dbResp = await httpClient.DeleteJsonAsync($"CVDocument/{e.Id}", new AuthenticationHeaderValue("Bearer", token));
                if (dbResp.IsSuccessStatusCode)
                {
                    // Give a pop up
                    Console.WriteLine("Entire Record has been deleted");
                    await retrievecvDocuments();
                }
                ///api/Document/{id} 
            }
        }
        private string matricCertContainer = "matric-certificates";//"images";
        private async Task uploadAdditionalDocuments(InputFileChangeEventArgs e)
        {
            // still need to check that files that has the same name aren't uploaded.
            fileNames = e.File;
            progressBar = 0.ToString("0");
            status = new StringBuilder($"Uploading file {num++}");
            var fileName = state.AppUserId + e.File.Name;

            foreach (var item in additionalUserDocuments)
            {
                if (item.documentName.Equals(e.File.Name))
                {
                    fileAlreadyExists = true;                    
                    break;
                }
                else
                {
                    fileAlreadyExists = false;
                }
            }            
            if (e.File.Size > maxAllowedSize)
            {
                await jsRuntime.InvokeAsync<object>("alert", "The document size is too large, please upload a document that is < 1mb");
            }
            else
            {
                if (fileAlreadyExists)
                {
                    await jsRuntime.InvokeAsync<object>("alert", "This Document already exists within our system");
                }
                else
                {
                    //Upload to blob - start
                    status = new StringBuilder($"current file {fileNames.Name}");

                    status.AppendLine("\n");

                    // Change the blobStorage location still
                    var blobUri = new Uri("https://"
                                          + storageAcc
                                          + ".blob.core.windows.net/"
                                          + matricCertContainer
                                          + "/"
                                          + fileName);
                    Console.WriteLine("fileName " + fileNames.Name);

                    AzureSasCredential credential = new AzureSasCredential(azureCredentials);

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

                        //var response = await httpClient.GetAsync("ResumeParser");
                        // var documentLocation = blobUri.ToString();

                        additionalDocUploadInfo.documentName = fileNames.Name;
                        additionalDocUploadInfo.documentBlobLink = blobUri.ToString();
                        additionalDocUploadInfo.AppUserId = state.AppUserId;


                        var resp = await httpClient.PostJsonAsync($"AdditionalDocument", additionalDocUploadInfo, new AuthenticationHeaderValue("Bearer", token));
                        if (resp.IsSuccessStatusCode)
                        {
                            //await OnInitializedAsync();
                            await jsRuntime.InvokeAsync<object>("alert", "Your Document has successfully been uploaded");
                            await retrieveAdditionalDocuments();
                        }
                    }
                }
            }
            
            
            enableUploadButtons = true;
        }

        async Task DeleteAdditionalDocuments(AdditionalDocument val)
        {
            
            var fileName = state.AppUserId + val.documentName;
            var blobUri = new Uri("https://"
                                  + storageAcc
                                  + ".blob.core.windows.net/"
                                  + matricCertContainer
                                  + "/"
                                  + fileName);
            // Console.WriteLine("fileName " + fileNames.Name);

            AzureSasCredential credential = new AzureSasCredential(azureCredentials);

            BlobClient blobClient = new BlobClient(blobUri, credential, new BlobClientOptions());
            status.AppendLine("Created blob client");

            status.AppendLine("\n");
            status.AppendLine("Sending to blob");
            //displayProgress = true;
            var res = await blobClient.DeleteIfExistsAsync();

            if (res.GetRawResponse().Status <= 205)
            {
                Console.WriteLine("Successfully deleted file from the blob");
                additionalUserDocuments.RemoveAll(x => x == val);
                var dbResp = await httpClient.DeleteJsonAsync($"AdditionalDocument/{val.Id}", new AuthenticationHeaderValue("Bearer", token));
                if (dbResp.IsSuccessStatusCode)
                {
                    Console.WriteLine("Entire Record has been deleted");
                    //await retrieveAdditionalDocuments();
                    if (additionalUserDocuments.Count == 0)
                    {
                        newAdditionalDocumentInfo = true;
                    }
                    else
                    {
                        newAdditionalDocumentInfo = false;
                    }
                }
                ///api/Document/{id} 
            }
        }


        private string userPicInfo;
        private string userImage = "profile-images";//"images";
        private async Task UploadingProfilePic(InputFileChangeEventArgs e)
        {
            // Getting the file
            var fileArray = e.File.Name.Split('.');
            userPicInfo = e.File.Name;
            var fileName = state.AppUserId;//fileArray[0] + Guid.NewGuid().ToString().Substring(0, 5) + "." + fileArray[1]; // change file name to be their appUserID
            var fileInfo = e.File;
            var fileType = e.File.ContentType;

            Console.WriteLine("File type: " + fileType);

            
                // You require a azure account with a storage account. You use that link for below. The 'images' is the file that the file image is stored in in Azure.
                // https://xebecstorage.blob.core.windows.net/profile-images

                var blobUri = new Uri("https://"
                    + storageAcc
                    + ".blob.core.windows.net/"
                    + userImage
                    + "/"
                    + fileName);

                AzureSasCredential credential = new AzureSasCredential(azureCredentials);
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
                    profilePic.AppUserId = state.AppUserId;
                    profilePic.profilePic = blobUri.ToString();
                    userPic = blobUri.ToString();
                    Console.WriteLine("Result is true whooooo");
                    var content = new FormUrlEncodedContent(new[]
                                    {
                                    new KeyValuePair<string, string>("url", $"{blobUri.ToString()}")
                                });

                    if (profilePictureExists)
                    {
                        var req = await httpClient.PutJsonAsync($"ProfilePicture/{profilePic.Id}", profilePic, new AuthenticationHeaderValue("Bearer", token));
                        var data = await req.Content.ReadAsStringAsync();
                        var rep = data.ToString();
                        Console.WriteLine("Value retrieved from uploading image: " + rep);
                        //var resp = req.Content.ReadAsStringAsync().Result;                    
                        //var returnVal = jsonResp.RootElement.GetProperty("profilePic");
                        //var result = returnVal.ToString();                    

                        await retrieveProfilePic();
                    }
                    else
                    {
                        var resp = await httpClient.PostJsonAsync($"ProfilePicture", profilePic, new AuthenticationHeaderValue("Bearer", token));
                        var data = await resp.Content.ReadFromJsonAsync<JsonElement>();
                        var result = data.GetProperty("profilePic").ToString();
                        Console.WriteLine("Value retrieved from uploading image: " + result);
                        // Conclusion.. The above statements only work for POST requests... You do get the information as required. Quite useful instead of having to call a GET method to get the profile pic
                        await retrieveProfilePic();
                    }
                    // var newresp = await HttpClient.PutAsJsonAsync($"personalinformation/{personalInfo.Id}", personalInfo); //{personalInfo.Id}                               
                }
                else
                {
                    Console.WriteLine("result is false :(");
                }
        }
        private void enterMarksPage(bool value)
        {
            enterMatricMarks = value;
        }


        private void addAnotherMark(bool value)
        {
            enterNewMark = value;
        }

        private void insertMarks()
        {
            matricInputs.Add(new()
            {
                SubjectName = marks.SubjectName,
                SubjectMark = marks.SubjectMark,
                AppUserId = state.AppUserId
            });
            matricMarksAdded.Add(new()
            {
                SubjectName = marks.SubjectName,
                SubjectMark = marks.SubjectMark,
                AppUserId = state.AppUserId
            });

            marks.SubjectName = string.Empty;
            marks.SubjectMark = 0;
            enterNewMark = true;

        }

        private async Task SaveMarks()
        {
            Console.WriteLine("disabling buttons");
            addMatricMarksPressed = true;

            foreach(var mark in matricMarksAdded)
            {
               var validUpload = await httpClient.PostJsonAsync($"matricMark", mark, new AuthenticationHeaderValue("Bearer", token));

                if (validUpload.IsSuccessStatusCode)
                {
                    validMarkUpload = true;
                }
                else
                {
                    validMarkUpload = false;
                }
            }

            if (validMarkUpload)
            {
                await jsRuntime.InvokeAsync<object>("alert", "Your Marks has successfully been captured");
            }
            addMatricMarksPressed = false;
            Console.WriteLine("enabling buttons");

            //just trying this, for some reason the buttons would be stuck 'disabled' even when addMatricMarksPressed = false
            StateHasChanged();

            matricMarksAdded.Clear();
            matricInputs.Clear();            

            matricInputs = await httpClient.GetListJsonAsync<List<matricMarks>>($"matricMark/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));            
        }

        private async void RemoveMark(matricMarks item)
        {

            if(matricMarksAdded.Exists(x => x.SubjectName.Equals(item.SubjectName)))
            {
                matricMarksAdded.Remove(item);
                matricInputs.Remove(item);
            }
            else
            {
                matricInputs.Remove(item);
                await httpClient.DeleteJsonAsync($"matricMark/{item.id}", new AuthenticationHeaderValue("Bearer", token));
            }            
        }

        private void toggleMagnifiedDocument(string documentType, bool val)
        {
            showMagnifiedDocument = val;
            selectedDocument = documentType;
        }

        private void toggleMagnifiedAdditionalDocuments(string documentLink, bool val)
        {
            showMagnifiedDocument = val;
            selectedDocument = documentLink;
        }

        private void testCheckBox()
        {
            Console.WriteLine("Check box val: " + checkTestVal);
        }
    }    
}