using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services.MockServices
{
    public class MockApplicantDataService : IApplicantDataService
    {
        private List<Applicant> _applicants;

        private IEnumerable<Applicant> Applicants
        {
            get
            {
                if (_applicants == null)
                    InitializeApplicants();
                return _applicants;
            }
        }

        public async Task<IEnumerable<Applicant>> GetAllApplicants()
        {
            return await Task.Run(() => Applicants);
        }

        public Task<IEnumerable<Applicant>> GetAllApplicantsByJobId(int jobId)
        {
            throw new NotImplementedException();
        }

        public async Task<Applicant> GetApplicantDetails(int applicantId)
        {
            return await Task.Run(() => { return Applicants.FirstOrDefault(e => e.Id == applicantId); });
        }

        public Task<Applicant> AddApplicant(Applicant applicant)
        {
            throw new NotImplementedException();
        }

        public Task DeleteApplicant(int applicantId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Applicant>> UpdateApplicant(Applicant applicant)
        {
            _applicants = _applicants.Select(i =>
            {
                if (i.Id == applicant.Id)
                {
                    i.CstComment = applicant.CstComment;
                    i.CstMark = applicant.CstMark;
                    i.InterviewComment = applicant.InterviewComment;
                    i.InterviewRating = applicant.InterviewRating;
                    i.Phase = applicant.Phase;
                }

                return i;
            }).ToList();

            return _applicants;
        }

        private void InitializeApplicants()
        {
            if (_applicants == null)
                if (_applicants == null)
                {
                    var phases = Enum.GetNames(typeof(ApplicationPhase));
                    var mockApplicants = new Faker<Applicant>()
                        .RuleFor(a => a.Id, f => f.IndexFaker)
                        .RuleFor(a => a.FirstName, f => f.Name.FirstName())
                        .RuleFor(a => a.LastName, f => f.Name.LastName())
                        .RuleFor(a => a.CstMark, f => f.Random.Number(100))
                        .RuleFor(a => a.CstComment, f => f.Rant.Review())
                        .RuleFor(a => a.InterviewRating, f => f.Random.Number(5))
                        .RuleFor(a => a.InterviewComment, f => f.Rant.Review("Person"))
                        .RuleFor(a => a.Phase, f => f.PickRandom(phases))
                        .RuleFor(a => a.Avatar, f => f.Person.Avatar);
                    _applicants = mockApplicants.Generate(20).ToList();
                }
        }
        
        public enum ApplicationPhase
        {
            //get desciption
            [Description("Application Sent")] Application,
            Testing,
            Interview_Staff,
            Interview_CEO,
            Interview_HR,
            Offer,
            Hired
        }
    }
}