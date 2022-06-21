using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
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
   

    public class SkillsInformation : ICloneable
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int AppUserId { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            SkillsInformation test = (SkillsInformation)obj;
            return string.Equals(this.Description, test.Description, StringComparison.OrdinalIgnoreCase);
        }
    }
    public class matricMarks
    {
        public int id { get; set; }
        [Required]
        public string SubjectName { get; set; }
        [Required]
        public int SubjectMark { get; set; }

        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
    public class currentPhase
    {
        public currentPhases[] phases { get; set; }
    }

    public class currentPhases
    {
        public int id { get; set; }
        public DateTime timeMoved { get; set; }
        public object comments { get; set; }
        public int rating { get; set; }
        public int applicationId { get; set; }
        public Application application { get; set; }
        public int applicationPhaseId { get; set; }
        public Applicationphase applicationPhase { get; set; }
    }

    public class Applicationphase
    {
        public int id { get; set; }
        public string description { get; set; }
        public string emailTemplate { get; set; }
        public object[] phaseHelpers { get; set; }
        public object jobPhases { get; set; }
    }


    public class References
    {
        public int id { get; set; }
        public string refFirstName { get; set; }
        public string refLastName { get; set; }
        public string refPhone { get; set; }
        public string refEmail { get; set; }
        public string relationship { get; set; }
        public int appUserId { get; set; }
        public AppUser appUser { get; set; }
    }

    public class Job
    {
        public int Id { get; set; } = 0;
        [Required]
        public string Title { get; set; } = String.Empty;
        [Required]
        public string Description { get; set; } = String.Empty;
        [Required]
        public int CompanyId { get; set; } = 0;
        public Company Company { get; set; } = new();
        [Required]
        public int PolicyId { get; set; } = 0;
        public Policy Policy { get; set; } = new();
        public decimal? Compensation { get; set; } = 0;
        public int? MinimumExperience { get; set; } = 0;
        [Required]
        public int LocationId { get; set; } = 0;

        public Location Location { get; set; } = new();
        public int DepartmentId { get; set; } = 0;
        public Department Department { get; set; } // Dont have to fill in this info when sending it to the DB
        public string Status { get; set; } = String.Empty;
        [Required]
        public DateTime DueDate { get; set; } = new();
        public DateTime CreationDate { get; set; } = new();
        //[Required]
        public List<JobTypeHelper> JobTypes { get; set; } = new();
        public List<JobPlatformHelper> JobPlatforms { get; set; } = new();
        public List<JobApplicationPhase> JobPhases { get; set; } = new();

        public List<Application> Applications { get; set; } = new();

    }

    public class Department
    {
        public int Id { get; set; } 
        public string Name { get; set; }
    }

    public class CreateJobPost
    {
        public int Id { get; set; } = 0;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int CompanyId { get; set; } = 0; // Remember to change this to string.Empty

        public Company Company { get; set; } = new();
        public decimal? Compensation { get; set; } = null;
        public int? MinimumExperience { get; set; } = 0;
        [Required]
        public int LocationId { get; set; } = 0; // Remember to change this to string.Empty

        public Location Location { get; set; } = new();
        //[Required]
        //public string Department { get; set; }
        [Required]
        public int PolicyId { get; set; } = 0;//string.Empty;// Not sure what data type it should be, but policy is a required field
                                              //
        public Policy Policy { get; set; } = new();
        public int JobTypeId { get; set; } = 0;
        public int DepartmentId { get; set; } = 0;// testing problem
        public string Status { get; set; }  = string.Empty;
        [Required]
        public DateTime DueDate { get; set; } = DateTime.Today.Date;
        public DateTime CreationDate { get; set; } = DateTime.Today.Date;
        public JobType JobType { get; set; } = new();
        public Department Department { get; set; } = new();
        public List<JobPlatform> JobPlatforms { get; set; } = new ();
        public List<ApplicationPhase> JobPhases { get; set; } = new();
        public List<FormQuestion> formQuestions { get; set; } = new();
        public List<AppUser> Collaborators { get; set; } = new();
        
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

        public string JobTitle { get; set; }

        public int JobId { get; set; }

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

        public string EmailTemplate { get; set; }
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

    public class EmailModel
    {
        public string CandidateFirstName { get; set; }
        public string CandidateLastName { get; set; }
        public string CandidateEmail { get; set; }
        public string JobName { get; set; }
        public string Body { get; set; }
    }

    public class HrEmailModel
    {
        public string CandidateName { get; set; }

        public string CandidateSurname { get; set; }

        public string AuthorizerEmail { get; set; }

        public string Phase { get; set; }

        public string Job { get; set; }

        public string DateTime { get; set; }
    }

    public class CollaboratorQuestion
    {
        public int Id { get; set; }

        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public int FormQuestionId { get; set; }

        public FormQuestion FormQuestion { get; set; }
    }

    public class Document
    {
        public int Id { get; set; } = 0;

        public string CV { get; set; } = String.Empty;

        public string MatricCertificate { get; set; } = String.Empty;

        public string UniversityTranscript { get; set; } = String.Empty;

        public string AdditionalCert1 { get; set; } = String.Empty;

        public string AdditionalCert2 { get; set; } = String.Empty;

        public string AdditionalCert3 { get; set; } = String.Empty;


        //Foreign Key: AppUser
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }

    public class QuestionnaireHrForm
    {
        public int Id { get; set; }
        public string Question { get; set; }

        public string Answer { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        public int AnswerTypeId { get; set; }
        public AnswerType AnswerType { get; set; }
    }

   public class PersonalInformation
    {
        public int Id { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser{ get; set; }
    }

    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Policy
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }


}