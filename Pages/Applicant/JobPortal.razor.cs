using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using X.PagedList;
using XebecPortal.UI.Pages.Applicant.Models;
using XebecPortal.UI.Shared;

namespace XebecPortal.UI.Pages.Applicant
{
    public partial class JobPortal
    {
        private string searchJob;
        private bool IsApplyHidden;
        private bool nextButton, preButton = true;
        private bool jobPortalIsHidden = false;
        private bool applicationFormIsHidden = true;
        private bool pageLoad;
        private int jobId;
        private IList<Job> jobList = new List<Job>();
        private IList<Job> jobListFilter = new List<Job>();
        private Job displayJobDetail = new Job();
        private IPagedList<Job> jobPagedList = new List<Job>().ToPagedList();
        private IList<Application> applicationList = new List<Application>();
        private bool JobPortalIsHidden, ApplicationFormIsHidden;
        private List<JobType> JobTypes;
        private List<Status> status;

        private IEnumerable<string> mudSelectLocation;
        private IEnumerable<string> mudSelectCompany;
        private IEnumerable<string> mudSelectDepartment;
        private IEnumerable<string> mudSelectStatus;

        protected override async Task OnInitializedAsync()
        {
            JobTypes = await httpClient.GetFromJsonAsync<List<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");
            jobList = await httpClient.GetFromJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            applicationList = await httpClient.GetFromJsonAsync<List<Application>>("https://xebecapi.azurewebsites.net/api/Application");
            status = await httpClient.GetFromJsonAsync<List<Status>>("/mockData/Status.json");

            jobListFilter = jobList;
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            displayJobDetail = jobListFilter.FirstOrDefault();
            DisplayJobDetail(displayJobDetail.Id);
            JobPortalIsHidden = false;
            ApplicationFormIsHidden = true;
            JobTypes = await httpClient.GetFromJsonAsync<List<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");
        }

        private async Task Apply(int id)
        {
            Application application = new()
            {
                TimeApplied = DateTime.Today,
                BeginApplication = DateTime.Today,
                JobId = id,
                AppUserId = 1
            };

            _ = await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/ApplicationModel", application);
            jobList = await httpClient.GetFromJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            applicationList = await httpClient.GetFromJsonAsync<List<Application>>("https://xebecapi.azurewebsites.net/api/Application");

            JobPortalIsHidden = true;
            ApplicationFormIsHidden = false;

            IsApplyHidden = true;
            Types = await httpClient.GetFromJsonAsync<List<QuestionType>>("https://xebecapi.azurewebsites.net/api/answertype");
            QuestionList = await httpClient.GetFromJsonAsync<List<FormQuestion>>($"https://xebecapi.azurewebsites.net/api/questionnaire/job/{id}");

            foreach (var q in QuestionList)
            {
                ApplicantQuestion tempAppQuestion = new ApplicantQuestion();
                tempAppQuestion.HRQuestion = q.question;
                tempAppQuestion.HRQuestionId = q.id;
                tempAppQuestion.TypeId = q.answerTypeId;

                ApplicantAnswers.Add(tempAppQuestion);
            }
        }

        private void PageListNav(int value)
        {
            jobPagedList = jobListFilter.ToPagedList(value, 17);
            nextButton = value == jobPagedList.PageCount || jobPagedList.PageCount == 1;
            preButton = value == 1;
        }

        private void SearchListJob(ChangeEventArgs e)
        {
            searchJob = e.Value.ToString();
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
        }

        private void SearchListLocation(IEnumerable<string> value)
        {
            mudSelectLocation = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
        }
        private void SearchListCompany(IEnumerable<string> value)
        {
            mudSelectCompany = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
        }

        private void SearchListDepartment(IEnumerable<string> value)
        {
            mudSelectDepartment = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
        }

        private void SearchListStatus(IEnumerable<string> value)
        {
            mudSelectStatus = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
        }

        private void DisplayJobDetail(int id)
        {
            IsApplyHidden = applicationList.Any(x => x.AppUserId == 1 && x.JobId == id);
            displayJobDetail = jobListFilter.FirstOrDefault(x => x.Id == id);
        }

        private void GoToApplicationForm()
        {
            applicationFormIsHidden = false;
            jobPortalIsHidden = true;
        }

        private string GetStyling(Job item)
        {
            if (displayJobDetail.Id == item.Id)
                return "box-shadow: rgba(0,51,64,0.86) 0px 0px 0px 2px, rgba(6, 24, 44, 0.65) 0px 4px 6px -1px, rgba(255, 255, 255, 0.08) 0px 1px 0px inset;";
            return "";
        }

        private string GetJobType(JobTypeHelper helper)
        {
            var type = new JobType { Id = 0, Type = "Contract" };

            if (JobTypes != null && helper != null)
            {
                type = JobTypes.FirstOrDefault(e => e.Id == helper.Id);
            }
            return ""; //type.Type;
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

        private static string GetMultiSelectionTextStatus(List<string> selectedValues)
        {
            return $"Selected Status{(selectedValues.Count > 1 ? "es" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private void FilterDataHelper()
        {
            if (!string.IsNullOrEmpty(searchJob) && searchJob != " ")
            {
                jobListFilter = jobListFilter.Where(x => x.Title.Contains(searchJob, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (mudSelectLocation?.Any() == true)
            {
                var listLocations = jobListFilter.Select(x => x.Location).Except(mudSelectLocation).ToList();
                jobListFilter = jobListFilter.Where(x => !listLocations.Contains(x.Location)).ToList();
            }

            if (mudSelectCompany?.Any() == true)
            {
                var listCompany = jobListFilter.Select(x => x.Company).Except(mudSelectCompany).ToList();
                jobListFilter = jobListFilter.Where(x => !listCompany.Contains(x.Company)).ToList();
            }

            if (mudSelectDepartment?.Any() == true)
            {
                var listDepartments = jobListFilter.Select(x => x.Department).Except(mudSelectDepartment).ToList();
                jobListFilter = jobListFilter.Where(x => !listDepartments.Contains(x.Department)).ToList();
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
            displayJobDetail = jobListFilter.FirstOrDefault();
        }

        public async Task SaveAnswers()
        {
            double tempScore;
            double tempMatches = 0;

            foreach (var q in ApplicantAnswers)
            {
                ApplicantAnswer tempAnswer = new ApplicantAnswer();
                tempAnswer.applicantAnswer = q.Applicantanswer;
                tempAnswer.questionnaireHRFormId = q.HRQuestionId;
                tempAnswer.appUserId = state.AppUserId;

                AnswerList.Add(tempAnswer);
            }

            await httpClient.PostAsJsonAsync<List<ApplicantAnswer>>("https://xebecapi.azurewebsites.net/api/applicantquestionnaire/list", AnswerList);

            for (int i = 0; i < QuestionList.Count; i++)
            {

                if (QuestionList[i].question.Contains("How many years of experience do you have"))
                {
                    if (int.Parse(QuestionList[i].answer) <= int.Parse(AnswerList[i].applicantAnswer))
                    {
                        tempMatches++;
                    }
                }
                else if (QuestionList[i].question == "What salary are you expecting?")
                {
                    if (int.Parse(QuestionList[i].answer) >= int.Parse(AnswerList[i].applicantAnswer))
                    {
                        tempMatches++;
                    }
                }
                else if (QuestionList[i].answer == AnswerList[i].applicantAnswer)
                {
                    tempMatches++;
                }
            }

            tempScore = tempMatches / QuestionList.Count * 100;

            CandidateRecommender candidateScore = new CandidateRecommender();
            candidateScore.jobId = jobId;
            candidateScore.AppUserId = state.AppUserId;
            candidateScore.TotalMatch = tempScore;

            //await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/applicantquestionnaire", candidateScore);
        }
    }
}