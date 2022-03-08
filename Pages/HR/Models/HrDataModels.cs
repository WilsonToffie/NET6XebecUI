using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Pages.HR
{
    internal class ApplicantData
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int cst_mark { get; set; }
        public string cst_comment { get; set; }
        public int interview_rating { get; set; }
        public string interview_comment { get; set; }
        public string phase { get; set; }
    }

    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public decimal? Compensation { get; set; }
        public int? MinimumExperience { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreationDate { get; set; }
        public List<JobTypeHelper> JobTypes { get; set; }
        public List<JobPlatformHelper> JobPlatforms { get; set; }
        public List<JobApplicationPhase> JobPhases { get; set; }
    }

    public class CreateJobPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public decimal? Compensation { get; set; }
        public int? MinimumExperience { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreationDate { get; set; }
        public JobType JobType { get; set; }
        public List<JobPlatform> JobPlatforms { get; set; }
        public List<ApplicationPhase> JobPhases { get; set; }
        public List<FormQuestion> formQuestions { get; set; }
        public List<AppUser> Collaborators { get; set; }
    }

    public partial class CandidateRecommender
    {
        public int id { get; set; }

        //Foreign Key
        public int jobId { get; set; }
        public Job job { get; set; }

        //Foreign Key
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public double TotalMatch { get; set; }
    }


    public class Collaborator
    {
        public int id { get; set; }
        public string Name { get; set; }
        //foreign key
        public int JobId { get; set; }
        public Job Job { get; set; }
        //foreign key
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }

    public class FormQuestion
    {
        public int id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public int jobId { get; set; }
        public Job job { get; set; }
        public int answerTypeId { get; set; }
        public AnswerType answerType { get; set; }
    }

    public class AnswerType
    {
        public int id { get; set; }
        public string Type { get; set; }
    }

    public class CustomQuestion
    {
        public int Id { get; set; }
        public string questionDescription { get; set; }
        public int answerTypeId { get; set; }
        public string answerType { get; set; }
    }

    public class JobType
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class JobTypeHelper
    {
        public int Id { get; set; }
        //Foreign Key: Job
        public int JobId { get; set; }
        public Job Job { get; set; }
        //Foreign Key: JobType
        public int JobTypeId { get; set; }
        public JobType JobType { get; set; }
    }

    public class ApplicationPhase
    {
        public int id { get; set; }

        public string description { get; set; }

        public string EmailTemplate { get; set; }
    }

    public class AppUser
    {
        public int id { get; set; }

        public string email { get; set; }

        public string role { get; set; }

        public string passwordhash { get; set; }

        public string name { get; set; }

        public string surname { get; set; }

        public string key { get; set; }

        public bool registered { get; set; }

        public int linkVisits { get; set; }
        public string imageUrl { get; set; }
    }

    public class JobPlatformHelper
    {
        public int Id { get; set; }
        //Foreign Key: Job
        public int JobId { get; set; }
        public Job Job { get; set; }
        //Foreign Key: JobPlatform
        public int JobPlatformId { get; set; }
        public JobPlatform JobPlatform { get; set; }
    }

    public class JobApplicationPhase
    {
        public int Id { get; set; }
        //Foreign Key: Job
        public int JobId { get; set; }
        public Job Job { get; set; }
        //Foreign Key: ApplicationPhase
        public int ApplicationPhaseId { get; set; }
        public ApplicationPhase ApplicationPhase { get; set; }
    }

    public class JobPlatform
    {
        public int id { get; set; }
        public string platformName { get; set; }
    }

    public class Application
    {
        public int Id { get; set; }

        public DateTime TimeApplied { get; set; }

        public DateTime BeginApplication { get; set; }

        public int JobId { get; set; }

        public Job Job { get; set; }

        public int ApplicationPhaseId { get; set; }

        public ApplicationPhase ApplicationPhase { get; set; }

        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }

    public class ApplicationPhasesHelper
    {
        public int Id { get; set; }

        public DateTime TimeMoved { get; set; }

        public string Comments { get; set; }

        public double Rating { get; set; }

        public int ApplicationId { get; set; }

        public Application Application { get; set; }

        public int ApplicationPhaseId { get; set; }

        public ApplicationPhase ApplicationPhase { get; set; }

        public int StatusId { get; set; }

        public Status Status { get; set; }
    }

    //public class Status
    //{
    //    public int Id { get; set; }

    //    public string Description { get; set; }
    //}

    public class ApplicationPhaseItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public int ApplicationId { get; set; }

        public int AppUserId { get; set; }

        public int ApplicationPhaseId { get; set; }

        public string Picture { get; set; }

        public string Email { get; set; }
    }

    public class UnsuccessfulReason
    {
        public int Id { get; set; }

        public string Reason { get; set; }
    }

    public class RejectedCandidate
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public Application Application { get; set; }

        public int UnsuccessfulReasonId { get; set; }

        public UnsuccessfulReason UnsuccessfulReason { get; set; }
    }

    public class Status
    {
        public string name { get; set; }
    }

    public class CollaboratorsAssigned
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
    }

}