using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services.MockServices
{
    public class MockMyJobListDataService : IMyJobListDataService
    {
        private List<MyJob> _myJobs;

        private List<MyJob> MyJob
        {
            get
            {
                if (_myJobs == null)
                    InitializeJobs();
                return _myJobs;
            }
        }

        private void InitializeJobs()
        {
            if (_myJobs == null)
            {
                var locations = Enum.GetNames(typeof(Locations));
                var phases = Enum.GetNames(typeof(Phases));
                var statuses = Enum.GetNames(typeof(Statuses));
                var mockJobs = new Faker<MyJob>()
                    .RuleFor(j => j.Id, f => f.IndexGlobal)
                    .RuleFor(j => j.Position, f => f.Name.JobTitle())
                    .RuleFor(j => j.Company, f => f.Company.CompanyName())
                    .RuleFor(j => j.Location, f => f.PickRandom(locations))
                    .RuleFor(j => j.Phase, f => f.PickRandom(phases))
                    .RuleFor(j => j.Status, f => f.PickRandom(statuses))
                    .RuleFor(j => j.LastMoved, f => f.Date.Soon(2))
                    .RuleFor(j => j.ApplicationDate, f => f.Date.Recent(10));
                _myJobs = mockJobs.Generate(20).ToList();
            }
        }

        public enum Locations
        {
            Gauteng, Kzn, WesternCape, Limpopo, NorthWest, Remote 
        }

        public enum Phases
        {
            Application, InterviewHr, InterviewStaff, Testing, Screening, Offer
        }

        public enum Statuses
        {
            InProgress, Rejected
        }
        public List<MyJob> GetAllJobs()
        {
            return MyJob;
        }

        public Task<List<MyJob>> GetAllJobsByAppUserId(int appUserId)
        {
            throw new System.NotImplementedException();
        }
    }
}