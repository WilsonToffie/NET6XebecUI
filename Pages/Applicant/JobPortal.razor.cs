using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using X.PagedList;
using XebecPortal.UI.Pages.Applicant.Models;
using XebecPortal.UI.Pages.HR;
using XebecPortal.UI.Shared;
using XebecPortal.UI.Utils.Handlers;

namespace XebecPortal.UI.Pages.Applicant
{
    public partial class JobPortal
    {
        private string token;
        private string searchJob;
        private bool submitModal = false;
        private bool IsApplyHidden;
        private bool nextButton, preButton = true;
        private bool jobPortalIsHidden = false;
        private bool applicationFormIsHidden = true;
        private bool pageLoad;
        private bool isFilterContainAnyVal;
        private int jobId;
        private IList<Job> jobList = new List<Job>();
        private int currentJob;
        private IList<Job> jobListFilter = new List<Job>();
        private Job displayJobDetail = new Job();
        private IPagedList<Job> jobPagedList = new List<Job>().ToPagedList();
        private IList<Application> applicationList = new List<Application>();
        private List<JobType> JobTypes;
        private List<JobTypeHelper> jobTypeHelper;
        private List<Status> status;
        private List<Department> departments;
        private List<FormQuestion> QuestionList = new List<FormQuestion>();
        private List<QuestionType> Types = new List<QuestionType>();
        private IList<ApplicantQuestion> Questions = new List<ApplicantQuestion>();
        private List<ApplicantAnswer> AnswerList = new List<ApplicantAnswer>();
        private List<CandidateRecommender> candidates = new List<CandidateRecommender>();
        private ApplicationModel application = new ApplicationModel();

        private IEnumerable<string> mudSelectLocation;
        private IEnumerable<string> mudSelectCompany;
        private IEnumerable<string> mudSelectDepartment;
        private IEnumerable<string> mudSelectJobType;

        private IJSObjectReference _jsModule;

        private bool invalidAnswers = false;
        protected override async Task OnInitializedAsync()
        {
            token = await localStorage.GetItemAsync<string>("jwt_token");
            QuestionList = await httpClient.GetListJsonAsync<List<FormQuestion>>("Questionnaire", new AuthenticationHeaderValue("Bearer", token));
            JobTypes = await httpClient.GetListJsonAsync<List<JobType>>("JobType", new AuthenticationHeaderValue("Bearer", token));
            jobList = await httpClient.GetListJsonAsync<List<Job>>("Job", new AuthenticationHeaderValue("Bearer", token));
            applicationList = await httpClient.GetListJsonAsync<List<Application>>($"Application/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));
            //status = await httpClient.GetListJsonAsync<List<Status>>("./mockData/Status.json", new AuthenticationHeaderValue("Bearer", token));
            jobTypeHelper = await httpClient.GetListJsonAsync<List<JobTypeHelper>>("JobTypeHelper", new AuthenticationHeaderValue("Bearer", token));
            candidates = await httpClient.GetListJsonAsync<List<CandidateRecommender>>("CandidateRecommender", new AuthenticationHeaderValue("Bearer", token));
            jobList = jobList.Where(x => x.Status == "Open").ToList();
            jobListFilter = jobList;
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            displayJobDetail = jobListFilter.FirstOrDefault();
            if (jobList.Count == 0)
            {
            }
            else
            {
                DisplayJobDetail(displayJobDetail.Id);

            }
            jobPortalIsHidden = false;
            applicationFormIsHidden = true;
            //JobTypes = await httpClient.GetListJsonAsync<List<JobType>>("JobType", new AuthenticationHeaderValue("Bearer", token));
            departments = await httpClient.GetListJsonAsync<List<Department>>("Department", new AuthenticationHeaderValue("Bearer", token));

            _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./jsPages/Applicant/JobPortalv1.js");

            

        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            _jsModule.InvokeVoidAsync("ResizeTextArea");
            return base.OnAfterRenderAsync(firstRender);
        }

        private async Task Apply(int id)
        {
            submitModal = true;
            Questions.Clear();
            jobId = id;
            application = new()
            {
                TimeApplied = DateTime.Now,
                JobId = id,
                AppUserId = state.AppUserId,
                ApplicationPhaseId = 1
            };

            //await httpClient.PostJsonAsync("Application", application, new AuthenticationHeaderValue("Bearer", token));

            jobList = await httpClient.GetListJsonAsync<List<Job>>("Job", new AuthenticationHeaderValue("Bearer", token));

            jobList = jobList.Where(x => x.Status == "Open").ToList();
            QuestionList = await httpClient.GetListJsonAsync<List<FormQuestion>>($"Questionnaire/Job/{id}", new AuthenticationHeaderValue("Bearer", token));
            applicationList = await httpClient.GetListJsonAsync<List<Application>>($"Application/all/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));

            jobPortalIsHidden = true;
            applicationFormIsHidden = false;

            Types = await httpClient.GetListJsonAsync<List<QuestionType>>("answertype", new AuthenticationHeaderValue("Bearer", token));

            foreach (var q in QuestionList)
            {
                ApplicantQuestion tempAppQuestion = new()
                {
                    HRQuestion = q.question,
                    HRQuestionId = q.id,
                    TypeId = q.answerTypeId
                };

                Questions.Add(tempAppQuestion);
            }
            submitModal = false;
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

        private async Task SearchListJobType(IEnumerable<string> value)
        {
            mudSelectJobType = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private void DisplayJobDetail(int id)
        {
            displayJobDetail = jobListFilter.FirstOrDefault(x => x.Id == id);
            currentJob = id;
        }

        private void GoToApplicationForm()
        {
            applicationFormIsHidden = false;
            jobPortalIsHidden = true;
        }

        private void ToJobPortal()
        {
            applicationFormIsHidden = true;
            jobPortalIsHidden = false;
        }

        private string GetStyling(Job item)
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

        private static string GetMultiSelectionTextJobType(List<string> selectedValues)
        {
            return $"Selected Job Type{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private void FilterDataHelper()
        {
            if (!string.IsNullOrEmpty(searchJob) && searchJob != " ")
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

            if (mudSelectJobType?.Any() == true)
            {
                var listjobTypeIds = JobTypes.Where(x => mudSelectJobType.Contains(x.Type)).Select(x => x.Id);
                var listJobIds = jobTypeHelper.Where(x => listjobTypeIds.Contains(x.JobTypeId)).Select(x => x.JobId);
                var exceptJobIds = jobListFilter.Select(x => x.Id).Except(listJobIds);
                jobListFilter = jobListFilter.Where(x => !exceptJobIds.Contains(x.Id)).ToList();
            }
        }

        private void FilterDataDisplayHelper()
        {
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            nextButton = jobPagedList.PageNumber == jobPagedList.PageCount;
            preButton = jobPagedList.PageNumber == 1;
            displayJobDetail = jobListFilter.FirstOrDefault();
        }

        public async Task SaveAnswers()
        {
            submitModal = true;

            double tempScore;
            double tempMatches = 0;
            
            foreach (var q in Questions)
            {
                if (q.Applicantanswer == null)
                {
                    await jsRuntime.InvokeAsync<object>("alert", "Please ensure that all of the answers are answered correctly!");
                    invalidAnswers = true;
                    break;
                }
                else
                {
                    ApplicantAnswer tempAnswer = new()
                    {
                        applicantAnswer = q.Applicantanswer,
                        questionnaireHRFormId = q.HRQuestionId,
                        appUserId = state.AppUserId
                    };

                    AnswerList.Add(tempAnswer);
                    }
            }

            if (!invalidAnswers)            
            {
                for (int i = 0; i < QuestionList.Count; i++)
                {
                    Console.WriteLine(QuestionList[i].answer + " " + AnswerList[i].applicantAnswer);
                    if (QuestionList[i].answer == AnswerList[i].applicantAnswer)
                    {
                        tempMatches++;
                    }
                }

                tempScore = tempMatches / QuestionList.Count * 100;
                Console.WriteLine(tempMatches + " / " + QuestionList.Count + " * " + 100);

                CandidateRecommender candidateRecommender = new CandidateRecommender();

                candidateRecommender.AppUserId = state.AppUserId;
                candidateRecommender.jobId = jobId;
                candidateRecommender.TotalMatch = tempScore;

                await httpClient.PostJsonAsync("CandidateRecommender", candidateRecommender, new AuthenticationHeaderValue("Bearer", token));

                await httpClient.PostJsonAsync("applicantquestionnaire/list", AnswerList, new AuthenticationHeaderValue("Bearer", token));

                application.BeginApplication = DateTime.Now;

                await httpClient.PostJsonAsync("Application", application, new AuthenticationHeaderValue("Bearer", token));

                List<ApplicationModel> applications = await httpClient.GetListJsonAsync<List<ApplicationModel>>("Application", new AuthenticationHeaderValue("Bearer", token));

                applications = applications.Where(x => x.AppUserId == application.AppUserId).ToList();
                applications = applications.OrderByDescending(x => x.BeginApplication).ToList();

                ApplicationPhasesHelper phaseHelper = new()
                {
                    TimeMoved = DateTime.Now,
                    Comments = "None",
                    Rating = 1,
                    ApplicationId = applications.FirstOrDefault().Id,
                    ApplicationPhaseId = 1
                };

                await httpClient.PostJsonAsync("ApplicationPhaseHelper", phaseHelper, new AuthenticationHeaderValue("Bearer", token));

                jobList = await httpClient.GetListJsonAsync<List<Job>>("Job", new AuthenticationHeaderValue("Bearer", token));
                applicationList = await httpClient.GetListJsonAsync<List<Application>>($"Application", new AuthenticationHeaderValue("Bearer", token));
                jobList = jobList.Where(x => x.Status == "Open").ToList();
                jobListFilter = jobList;
                ToJobPortal();
            }
            invalidAnswers = false;
            submitModal = false;
        }
    }
}