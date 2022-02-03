using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class ApplicationPhaseHelperDataService : IApplicationPhaseHelperDataService
    {
        private readonly HttpClient _httpClient;
        private HttpClient AltClient = new HttpClient();
        private List<ApplicationPhaseHelper> _mockPhaseHelpers;

        public ApplicationPhaseHelperDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        

        public async Task<List<ApplicationPhaseHelper>> GetAllApplicationPhaseHelpers()
        {
            var phaseHelpers = await JsonSerializer.DeserializeAsync<List<ApplicationPhaseHelper>>(
                utf8Json: await AltClient.GetStreamAsync($"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper"),
                options: new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            return phaseHelpers;
        }
        //api/ApplicationPhaseHelper/UserId={AppUserId}
        public List<ApplicationPhaseHelper> GetApplicationPhaseHelpersByUserId(int appUserId)
        {
            
            // return (await JsonSerializer.DeserializeAsync<List<ApplicationPhaseHelper>>
            //     (await AltClient.GetStreamAsync(
            //         $"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/userId={appUserId}"),
            //     new JsonSerializerOptions()
            //     {
            //         PropertyNameCaseInsensitive = true
            //     }));
            Console.WriteLine($">>>>>ApplicationPhaseHelperDataService : getting helper for {appUserId}");
             return Task.FromResult((AltClient.GetFromJsonAsync<List<ApplicationPhaseHelper>>(
                 $"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/userId={appUserId}"))).Result.Result;
        }
        
        List<ApplicationPhaseHelper> GetMockApplicationHelper(List<Applicant> applicants)
        {
            if (_mockPhaseHelpers == null)
                InitializeMockPhaseHelper(applicants);
            return _mockPhaseHelpers;
        }

        public List<ApplicationPhaseHelper> GetAssApplicationPhaseHelpers(Applicant applicant, List<Applicant> applicants)
        {
            var allHelpers = GetMockApplicationHelper(applicants);
            //Console.WriteLine($">>>>>>>>ApplicationPhaseHelper.cs finding helpers for applicant Id:{applicant.Id} ");
            var assHelpers =  allHelpers.FindAll(a => a.AppUserId == applicant.Id);
            //Console.WriteLine($">>>>>>>>ApplicationPhaseHelper.cs applicant Id:{applicant.Id} has {assHelpers.Count} helper allHelpers count = {allHelpers.Count}");
            return assHelpers;
        }

        private void InitializeMockPhaseHelper(List<Applicant> applicants)
        {
            _mockPhaseHelpers = new();
            //Create mock job list
            List<JobModel> jobs = GetMockJobs(50);
            //for each job simulate an application (create application)
            List<Application> applications = GetMockApplications(jobs, applicants, 60);
            
            
            var tempPhase = new AppPhase {Id = (int) PhaseEnum.Application, Description = "Application"};
            var tempStatus = new AppStatus {Id = (int) StatusEnum.InProgress, Description = "In Progress"};

            //for each application create phase helper
            for (int i = 0; i < applications.Count; i++)
            {
                ApplicationPhaseHelper tempHelper = new();
                var tempApplication = applications[i];
                tempHelper.Id = i;
                tempHelper.ApplicationId = applications[i].Id;
                tempHelper.Application = applications[i];
                tempHelper.ApplicationPhaseId = tempPhase.Id;
                tempHelper.ApplicationPhase = tempPhase;
                tempHelper.StatusId = tempStatus.Id;
                tempHelper.Status = tempStatus;
                tempHelper.AppUserId = applications[i].AppUserId;
                _mockPhaseHelpers.Add(tempHelper);
            }
        }

        private List<Application> GetMockApplications(List<JobModel> jobs, List<Applicant> applicants,int num)
        {
            var faker = new Faker<Application>()
                .RuleFor(application => application.Id, f => f.IndexFaker)
                .RuleFor(application => application.Job, f => f.PickRandom(jobs))
                .RuleFor(application => application.AppUser, f => f.PickRandom(applicants))
                .RuleFor(application => application.TimeApplied, f => f.Date.Recent(150))
                .RuleFor(application => application.BeginApplication, f => f.Date.Recent(10));

            return faker.Generate(num);
        }

        public List<JobModel> GetMockJobs(int num)
        {
            var faker = new Faker<JobModel>()
                .RuleFor(job => job.Id, f => f.IndexFaker)
                .RuleFor(job => job.Title, f => f.Name.JobTitle())
                .RuleFor(job => job.Company, f => f.Company.CompanyName());

            return faker.Generate(num);
        }
        public List<JobModel> GetMockApplicants(int num)
        {
            var faker = new Faker<JobModel>()
                .RuleFor(job => job.Id, f => f.IndexFaker)
                .RuleFor(job => job.Title, f => f.Name.JobTitle())
                .RuleFor(job => job.Company, f => f.Company.CompanyName());

            return faker.Generate(num);
        }
    }
}