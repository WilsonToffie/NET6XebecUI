using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using XebecPortal.UI.Pages.HR;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services.MockServices
{
    public static class AMockDataHub
    {
        public enum ApplicationPhase
        {
            //get desciption
            [Description("ApplicationModel Sent")] Application,
            Testing,
            InterviewStaff,
            InterviewCeo,
            Offer,
            Hired
        }

        public enum Status
        {
            Inprogress,
            Rejected
        }

        public static readonly List<Job> MockJobs = GetMockJobs().Generate(20);
        public static readonly List<JobModel> MockJobModels = GetMockJobModels(50);

        public static readonly List<Services.Models.AppUser> MockAppUsers = GetMockAppUsers().Generate(20);
       // public static readonly List<ApplicationModel> MockApplications = GetMockApplications().Generate(50);
        public static List<Applicant> MockApplicants = new();

        
        

        private static Faker<Services.Models.AppUser> GetMockAppUsers()
        {
            string[] roles = {"candidate", "developer", "hr"};
            return new Faker<Services.Models.AppUser>()
                .RuleFor(user => user.Id, f => f.IndexFaker)
                .RuleFor(user => user.Name, f => f.Name.FirstName())
                .RuleFor(user => user.Surname, f => f.Name.LastName())
                .RuleFor(user => user.Role, f => "candidate");
        }

        private static Faker<ApplicationModel> GetMockApplications()
        {
            return new Faker<ApplicationModel>()
                .StrictMode(true)
                .RuleFor(a => a.Id, f => f.IndexFaker)
                .RuleFor(a => a.JobId, f => f.PickRandom(MockJobs).Id)
                .RuleFor(a => a.AppUserId, f => f.PickRandom(MockApplicants).Id)
                .RuleFor(a => a.TimeApplied, f => f.Date.Recent(150));
        }

        private static Faker<Job> GetMockJobs()
        {
            return new Faker<Job>()
                    .RuleFor(job => job.Id, f => f.IndexFaker)
                    .RuleFor(job => job.Title, f => f.Name.JobTitle())
                    .RuleFor(job => job.Description, f => f.Name.JobDescriptor())
                    
                ;
        }

        public static List<ApplicationPhaseHelper> _mockPhaseHelpers;
        public static List<ApplicationPhaseHelper> GetAssApplicationPhaseHelpers(Applicant applicant, List<Applicant> applicants)
        {
            var allHelpers = GetMockApplicationHelper(applicants);
            //Console.WriteLine($">>>>>>>>ApplicationPhaseHelper.cs finding helpers for applicant Id:{applicant.Id} ");
            var assHelpers =  allHelpers.FindAll(a => a.AppUserId == applicant.Id);
            //Console.WriteLine($">>>>>>>>ApplicationPhaseHelper.cs applicant Id:{applicant.Id} has {assHelpers.Count} helper allHelpers count = {allHelpers.Count}");
            return assHelpers;
        }
        public static List<ApplicationPhaseHelper> GetMockApplicationHelper(List<Applicant> applicants)
        {
            if (_mockPhaseHelpers == null)
                InitializeMockPhaseHelper(applicants);
            return _mockPhaseHelpers;
        }
        private static void InitializeMockPhaseHelper(List<Applicant> applicants)
        {
            MockApplicants = applicants;
            _mockPhaseHelpers = new();
            //Create mock job list
            List<JobModel> jobs = MockJobModels;
            //for each job simulate an applicationModel (create applicationModel)
            List<ApplicationModel> applications = GetMockApplications(jobs, applicants, 60);
            foreach (var application in applications)
            {
                //Console.WriteLine($">>>>>>>>ApplicationPhaseHelper.cs InitializeMockPhaseHelper applications:{applicationModel.AppUserId} applicationModel.Job.Title:{applicationModel.Job.Title}");
            }
            
            
            var tempPhase = new AppPhase {Id = (int) PhaseEnum.Application, Description = "ApplicationModel"};
            var tempStatus = new AppStatus {Id = (int) StatusEnum.InProgress, Description = "In Progress"};

            //for each applicationModel create phase helper
            for (int i = 0; i < applications.Count; i++)
            {
                ApplicationPhaseHelper tempHelper = new();
                var tempApplication = applications[i];
                tempHelper.Id = i;
                tempHelper.ApplicationId = applications[i].Id;
                tempHelper.ApplicationModel = applications[i];
                tempHelper.ApplicationPhaseId = tempPhase.Id;
                tempHelper.ApplicationPhase = tempPhase;
                tempHelper.StatusId = tempStatus.Id;
                tempHelper.Status = tempStatus;
                
                //Console.WriteLine($">>>>>>>>ApplicationPhaseHelper.cs InitializeMockPhaseHelper tempHelper.AppUserId ");
                
                tempHelper.AppUserId = applications[i].AppUser.Id;
                //Console.WriteLine($">>>>>>>>ApplicationPhaseHelper.cs InitializeMockPhaseHelper tempHelper.AppUserId:{tempHelper.Id} applications[i].AppUserId:{applications[i].AppUserId}");
                _mockPhaseHelpers.Add(tempHelper);
            }
            
        }
        private static List<JobModel> GetMockJobModels(int num)
        {
            var faker = new Faker<JobModel>()
                .RuleFor(job => job.Id, f => f.IndexFaker)
                .RuleFor(job => job.Title, f => f.Name.JobTitle())
                .RuleFor(job => job.Company, f => f.Company.CompanyName());

            return faker.Generate(num);;
        }
        private static List<ApplicationModel> GetMockApplications(List<JobModel> jobs, List<Applicant> applicants,int num)
        {
            var faker = new Faker<ApplicationModel>()
                .RuleFor(application => application.Id, f => f.IndexFaker)
                .RuleFor(application => application.Job, f => f.PickRandom(jobs))
                .RuleFor(application => application.AppUser, f => f.PickRandom(applicants))
                .RuleFor(application => application.TimeApplied, f => f.Date.Recent(150))
                .RuleFor(application => application.BeginApplication, f => f.Date.Recent(10));

            return faker.Generate(num);
        }
    }
}