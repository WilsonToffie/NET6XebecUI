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
        private int jobId;
        private List<int> pageNum = new List<int>();
        private IList<Job> jobList = new List<Job>();
        private IList<Job> jobListFilter = new List<Job>();
        private Job displayJobDetail = new Job();
        private IPagedList<Job> jobPagedList = new List<Job>().ToPagedList();
        private IList<Application> applicationList = new List<Application>();
        private bool JobPortalIsHidden, ApplicationFormIsHidden;
        private List<JobType> JobTypes;
        private List<Job> Jobs = null;
        private List<QuestionType> Types = null;

        private List<FormQuestion> QuestionList = new List<FormQuestion>();
        private List<ApplicantQuestion> ApplicantAnswers = new List<ApplicantQuestion>();
        private List<ApplicantAnswer> AnswerList = new List<ApplicantAnswer>();

        protected override async Task OnInitializedAsync()
        {
            JobTypes = await httpClient.GetFromJsonAsync<List<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");
            jobList = await httpClient.GetFromJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            applicationList = await httpClient.GetFromJsonAsync<List<Application>>("https://xebecapi.azurewebsites.net/api/Application");


            jobListFilter = jobList;
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            displayJobDetail = jobListFilter.FirstOrDefault();
            DisplayJobDetail(displayJobDetail.Id);
            JobPortalIsHidden = false;
            ApplicationFormIsHidden = true;
            JobTypes = await httpClient.GetFromJsonAsync<List<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");
        }

        private async Task Apply(int id)
        {
            jobId = id;

            Application application = new Application();

            application.TimeApplied = DateTime.Today;
            application.BeginApplication = DateTime.Today;
            application.JobId = id;
            application.AppUserId = state.AppUserId;

            _ = await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/Application", application);
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
            jobPagedList = jobList.ToPagedList(value, 17);
        }

        private void ToJobPortal()
        {
            JobPortalIsHidden = false;
            ApplicationFormIsHidden = true;
        }
        private void SeachListJob(string value)
        {
            pageNum.Clear();
            
            if (value != null && value != "" && value != " ")
            {

                jobListFilter = jobList.Where(x => $"{x.Title} {x.Company} {x.Location}".Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
                jobPagedList = jobListFilter.ToPagedList(1, 17); 
                
                pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            }
            
            else
            {
                jobListFilter = jobList;
                jobPagedList = jobListFilter.ToPagedList(1, 17);
                pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            }
            
            displayJobDetail = jobListFilter.FirstOrDefault();
        }

        private void DisplayJobDetail(int id)
        {
            if (applicationList.Count(x => x.AppUserId == 1 && x.JobId == id) > 0)
                IsApplyHidden = true;
            else
                IsApplyHidden = false;

            displayJobDetail = jobListFilter.FirstOrDefault(x => x.Id == id);
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
