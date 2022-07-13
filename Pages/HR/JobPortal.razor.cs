using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using X.PagedList;
using XebecPortal.UI.Utils.Handlers;
namespace XebecPortal.UI.Pages.HR
{
    public partial class JobPortal
    {
        private bool changeForm;
        private bool isDialogVisible;
        private string searchJob;
        private string token;
        private bool nextButton, preButton = true;
        private bool isFilterContainAnyVal;
        private List<int> pageNum = new List<int>();
        private IList<Job> jobList = new List<Job>();
        private IList<Job> jobListFilter = new List<Job>();
        private Job displayJobDetail = new Job();
        private IPagedList<Job> jobPagedList = new List<Job>().ToPagedList();
        private IList<JobPlatform> jobPlatforms = new List<JobPlatform>();
        private IList<JobPlatformHelper> jobPlatformHelpers = new List<JobPlatformHelper>();
        private List<JobPlatform> platformsUsed = new List<JobPlatform>();
        private List<JobType> JobTypes;
        private List<JobTypeHelper> JobType;
        private List<JobTypeHelper> jobTypeHelper;
        private List<Status> status;
        private List<AppUser> appUser;
        private List<AppUser> appUserFilter;
        private List<CollaboratorsAssigned> collaboratorsAssigned;
        private List<CollaboratorsAssigned> collaboratorsAssigned2;
        private List<PersonalInformation> personalInformation;
        private List<Application> applicantionLists;
        //private JobTypeHelper newJobTypeHelper = new JobTypeHelper();
        private List<string> statuses = new() { "Draft", "Open", "Closed"};
        private List<Department> departments;
        private Department department;
        private List<string> locations = new() { "Eastern Cape", "Free State", " Gauteng", "KwaZulu-Natal", "Limpopo", "Mpumalanga", "Northen Cape", "North West", "Western Cape" };
        private MudBlazor.DialogOptions options = new() { CloseButton = true, FullWidth = true};
        private List<ProfilePicture> userProfilePicture = new List<ProfilePicture>();
        private IEnumerable<string> mudSelectLocation;
        private IEnumerable<string> mudSelectCompany;
        private IEnumerable<string> mudSelectDepartment;
        private IEnumerable<string> mudSelectStatus;

        private bool ShowingJobPortal = true;
        private bool ShowingApplicantPortal;
        private bool ShowingPhaseManager;
        private bool loadInfo = false;
        private string jobTypeString;

        private IJSObjectReference _jsModule;
        private string defaultCollaboratorImage = "/Img/DefaultImage.png";
        private DateTime today = DateTime.Today;
        private bool onErrorEvent = false;
        private int totalDays;
        private int count;
        private int openJobCounter;
        private int totalJobs;
        private string selectedFilterStatus;
        protected override async Task OnInitializedAsync()
        {
            openJobCounter = 0;           
            loadInfo = true;
            onErrorEvent = false;
            token = await localStorage.GetItemAsync<string>("jwt_token");
           
            jobList = await httpClient.GetListJsonAsync<List<Job>>($"Job", new AuthenticationHeaderValue("Bearer", token));
            applicantionLists = await httpClient.GetListJsonAsync<List<Application>>($"Application", new AuthenticationHeaderValue("Bearer", token));
            collaboratorsAssigned = await httpClient.GetListJsonAsync<List<CollaboratorsAssigned>>($"CollaboratorsAssigned", new AuthenticationHeaderValue("Bearer", token));
            appUser = await httpClient.GetListJsonAsync<List<AppUser>>($"User", new AuthenticationHeaderValue("Bearer", token));
            //JobTypes = await httpClient.GetListJsonAsync<List<JobType>>($"JobType", new AuthenticationHeaderValue("Bearer", token));
            //ShowJobPortal();
            //jobPlatforms = await httpClient.GetListJsonAsync<List<JobPlatform>>($"jobplatform", new AuthenticationHeaderValue("Bearer", token));
            //jobPlatformHelpers = await httpClient.GetListJsonAsync<List<JobPlatformHelper>>($"jobplatformhelper", new AuthenticationHeaderValue("Bearer", token));
            //jobTypeHelper = await httpClient.GetListJsonAsync<List<JobTypeHelper>>($"JobTypeHelper", new AuthenticationHeaderValue("Bearer", token));


            //personalInformation = await httpClient.GetListJsonAsync<List<PersonalInformation>>("PersonalInformation", new AuthenticationHeaderValue("Bearer", token));
            //userProfilePicture = await httpClient.GetListJsonAsync<List<ProfilePicture>>($"ProfilePicture", new AuthenticationHeaderValue("Bearer", token));
            ////status = await httpClient.GetFromJsonAsync<List<Status>>("/mockData/Status.json");
            //departments = await httpClient.GetFromJsonAsync<List<Department>>("department");
            //totalJobs = jobList.Count;

            //foreach (var item in appUser)
            //{
            //    foreach (var profilepic in userProfilePicture)
            //    {
            //        if (item.id == profilepic.AppUserId)
            //        {
            //            item.imageUrl = profilepic.profilePic;
            //            break;
            //        }
            //    }

            //}

            _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "/jsPages/HR/JobPortalv3.js");

            jobListFilter = jobList; //await httpClient.GetListJsonAsync<List<Job>>($"Job", new AuthenticationHeaderValue("Bearer", token));
            
            //jobPagedList = jobListFilter.ToPagedList(1, 17);
            totalJobs = jobListFilter.Count;
            foreach (var item in jobListFilter)
            {
                if (item.Status.Equals("Open"))
                {
                    openJobCounter++;
                }
            }
            //displayJobDetail = jobListFilter.FirstOrDefault();
            //if (jobList.Count == 0)
            //{
            //}
            //else 
            //{ 
            //    DisplayJobDetail(displayJobDetail.Id);
            //    OpenJobCollabToolTip(displayJobDetail.Id);
            //}
            loadInfo = false;
        }

        private void filterJobStatus(string filterValue)
        {
            Console.WriteLine("String value: " + filterValue.ToString());
            if (filterValue.Equals("Draft & Open"))
            {
                jobListFilter = jobList;
            }
            else
            {
                jobListFilter = jobList.Where(x => x.Status.Equals(filterValue)).ToList();
            } 
            
        }
        private int calculateDays(DateTime value)
        {
            
            // string val = value.ToString("d MMMM YYYY");
            // (today - val).Total
            return ((int)(today - value).TotalDays);
        }

        private int calculateTotalApplicants(int jobId)
        {
            count = 0;
            foreach(var item in applicantionLists)
            {
                if (item.JobId == jobId)
                {
                    count++;
                }
            }
            return count;
        }

        private void showJobDetails(int jobId)
        {
          // nav.NavigateTo("/applicantportal");
        }

        private string getHiringlead(int jobId)
        {
            foreach (var item in collaboratorsAssigned)
            {
                if (item.JobId == jobId)
                {
                    if (item.Name.Equals("Hiring Lead"))
                    {                        
                        foreach (var user in appUser)
                        {
                            if (user.id == item.AppUserId)
                            {
                                return user.name + " " + user.surname;                                
                            }
                        }
                    }
                }
            }
            return "There is no hiring lead for this job";
        }
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            jsRuntime.InvokeVoidAsync("HrJobPortalJS");
            _jsModule.InvokeVoidAsync("ResizeTextArea");

            return base.OnAfterRenderAsync(firstRender);
        }

        private void ShowApplicantPortal(int id)
        {
            hrJobState.JobId = id;
            ShowingApplicantPortal = true;
            ShowingJobPortal = false;
            ShowingPhaseManager = false;
        }

        private void ShowPhaseManager()
        {
            ShowingApplicantPortal = false;
            ShowingJobPortal = false;
            ShowingPhaseManager = true;
        }

        private void ShowJobPortal()
        {
            
            ShowingApplicantPortal = false;
            ShowingJobPortal = true;
            ShowingPhaseManager = false;
        }

        private void pageRedirect()
        {
            nav.NavigateTo("/createjob");
        }

        private void FormType(bool value)
        {
            changeForm = value;
        }

        private async Task DeleteData(int id)
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Delete This Item?"))
            {
                await httpClient.DeleteJsonAsync($"Job/{id}", new AuthenticationHeaderValue("Bearer", token));
                jobList = await httpClient.GetListJsonAsync<List<Job>>("Job", new AuthenticationHeaderValue("Bearer", token));
                jobListFilter = jobList;
                pageNum.Clear();
                jobPagedList = jobListFilter.ToPagedList(1, 17);
                displayJobDetail = jobListFilter.FirstOrDefault();
                pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            }
        }

        private async Task SaveData(Job jobValue, string jobTypeHelperValue, bool boolValue)
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Override This Item?"))
            {
                await _jsModule.InvokeVoidAsync("CursorWait");

                if (jobTypeString == null)
                {
                    jobTypeString = GetJobType(displayJobDetail.Id);
                }

                var newJobTypeHelper = jobTypeHelper.Find(x => x.JobId == displayJobDetail.Id);
                newJobTypeHelper.JobTypeId = JobTypes.Find(x => x.Type == jobTypeString).Id;
                newJobTypeHelper.JobType = JobTypes.Find(x => x.Type == jobTypeString);

                jobValue.JobTypes.Add(newJobTypeHelper);

                await httpClient.PutJsonAsync($"Job/{jobValue.Id}", jobValue, new AuthenticationHeaderValue("Bearer", token));
                await httpClient.PutJsonAsync($"JobTypeHelper/{newJobTypeHelper.Id}", newJobTypeHelper, new AuthenticationHeaderValue("Bearer", token));
                changeForm = boolValue;
                await _jsModule.InvokeVoidAsync("CursorDefault");
            }
        }

        private async Task PageListNav(int value)
        {
            jobPagedList = jobListFilter.ToPagedList(value, 17);
            nextButton = value == jobPagedList.PageCount || jobPagedList.PageCount == 1;
            preButton = value == 1;
            await _jsModule.InvokeVoidAsync("Scroll");
            DisplayJobDetail(jobPagedList.FirstOrDefault().Id);
        }

        private async Task SearchListJob(ChangeEventArgs e)
        {
            searchJob = e.Value.ToString();
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private async Task SearchListLocation(IEnumerable<string> value)
        {
            mudSelectLocation = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private async Task SearchListCompany(IEnumerable<string> value)
        {
            mudSelectCompany = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private async Task SearchListDepartment(IEnumerable<string> value)
        {
            mudSelectDepartment = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private async Task SearchListStatus(IEnumerable<string> value)
        {
            mudSelectStatus = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private void DisplayJobDetail(int id)
        {
            hrJobState.JobId = id;
            displayJobDetail = jobListFilter.FirstOrDefault(x => x.Id == id);
            platformsUsed.Clear();
        }

        private static string GetMultiSelectionTextLocation(List<string> selectedValues)
        {
            return $"Selected Location{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private static string GetMultiSelectionTextCompany(List<string> selectedValues)
        {
            return $"Selected Compan{(selectedValues.Count > 1 ? "ies" : "y")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private string GetMultiSelectionTextDepartment(List<string> selectedValues)
        {
            return $"Selected Department{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private static string GetMultiSelectionTextStatus(List<string> selectedValues)
        {
            return $"Selected Status{(selectedValues.Count > 1 ? "es" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private void FilterDataHelper()
        {
            if (!string.IsNullOrEmpty(searchJob))
            {
                jobListFilter = jobListFilter.Where(x => x.Title.Contains(searchJob, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (mudSelectLocation?.Any() == true)
            {
                var listLocations = jobListFilter.Select(x => x.Location.Name).Except(mudSelectLocation).ToList();
                jobListFilter = jobListFilter.Where(x => !listLocations.Contains(x.Location.Name)).ToList();
            }

            if (mudSelectCompany?.Any() == true)
            {
                var listCompany = jobListFilter.Select(x => x.Company.Name).Except(mudSelectCompany).ToList();
                jobListFilter = jobListFilter.Where(x => !listCompany.Contains(x.Company.Name)).ToList();
            }

            if (mudSelectDepartment?.Any() == true)
            {
                var listDepartments = jobListFilter.Select(x => x.Department.Name).Except(mudSelectDepartment).ToList();
                jobListFilter = jobListFilter.Where(x => !listDepartments.Contains(x.Department.Name)).ToList();
            }

            if (mudSelectStatus?.Any() == true)
            {
                var listStatus = jobListFilter.Select(x => x.Status).Except(mudSelectStatus).ToList();
                jobListFilter = jobListFilter.Where(x => !listStatus.Contains(x.Status)).ToList();
            }
        }

        private void FilterDataDisplayHelper()
        {
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            nextButton = jobPagedList.PageNumber == jobPagedList.PageCount;
            preButton = jobPagedList.PageNumber == 1;
            jsRuntime.InvokeVoidAsync("");
            displayJobDetail = jobListFilter.FirstOrDefault();
        }

        private object GetStyling(Job item)
        {
            if (displayJobDetail.Id == item.Id)
                return "box-shadow: rgba(0,51,64,0.86) 0px 0px 0px 2px, rgba(6, 24, 44, 0.65) 0px 4px 6px -1px, rgba(255, 255, 255, 0.08) 0px 1px 0px inset;";
            return "";
        }

        private string GetJobType(int id)
        {
            int jobTypeId = jobTypeHelper.Find(x => x.JobId == id).JobTypeId;
            return JobTypes.Find(x => x.Id == jobTypeId).Type;
        }

        private void OpenJobCollabDialog(int id)
        {
            isDialogVisible = true;

            var collabId = collaboratorsAssigned.Where(x => x.JobId == id).Select(x => x.AppUserId);
            appUserFilter = appUser.Where(x => collabId.Contains(x.id) && x.role != "Candidate").ToList();
        }

        private void OpenJobCollabToolTip(int id)
        {
            var collabId = collaboratorsAssigned.Where(x => x.JobId == id).Select(x => x.AppUserId);
            appUserFilter = appUser.Where(x => collabId.Contains(x.id) && x.role != "Candidate").ToList();
        }
        private async Task imageHandling()
        {
            onErrorEvent = true;
            await jsRuntime.InvokeAsync<string>("alert", "Error at loading user images, default image will be used!");
        }

    }


}