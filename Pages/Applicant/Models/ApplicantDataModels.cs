using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using XebecPortal.UI.Pages.HR;

namespace XebecPortal.UI.Pages.Applicant
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

    public class RejectedCandidate
    {
        public int id { get; set; }
        public int applicationId { get; set; }
        public Application application { get; set; }
        public int unsuccessfulReasonId { get; set; }
        public UnsuccessfullReasons unsuccessfulReason { get; set; }
    }


    public class UnsuccessfullReasons
    {
        public int id { get; set; }
        public string reason { get; set; }
        public object emailTemplate { get; set; }
        public object _RejectedCandidates { get; set; }
    }


    public class ApplicationPhase
    {
        public int id { get; set; }

        public string description { get; set; }

        public string EmailTemplate { get; set; }
    }

    public class ApplicationModel
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("jobId")] public int JobId { get; set; }

        [JsonProperty("job")] public Job Job { get; set; }

        [JsonProperty("appUserId")] public int AppUserId { get; set; }

        [JsonProperty("appUser")] public AppUser AppUser { get; set; }

        [JsonProperty("timeApplied")] public DateTimeOffset TimeApplied { get; set; }

        [JsonProperty("beginApplication")] public DateTime BeginApplication { get; set; }

        [JsonProperty("applicationPhaseId")]
        public int ApplicationPhaseId { get; set; }

        [JsonProperty("applicationPhase")]
        public ApplicationPhase ApplicationPhase { get; set; }

        [JsonProperty("phaseHelpers")]
        public ApplicationPhasesHelper applicationPhasesHelper { get; set; }
    }
    public class Job
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }
        public decimal? Compensation { get; set; }
        public int? MinimumExperience { get; set; }
        [Required]
        public int LocationId { get; set; }

        public Location Location { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; } // Dont have to fill in this info when sending it to the DB
        public string Status { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public DateTime CreationDate { get; set; }
        //[Required]
        public List<JobTypeHelper> JobTypes { get; set; }
        public List<JobPlatformHelper> JobPlatforms { get; set; }
        public List<JobApplicationPhase> JobPhases { get; set; }

        public List<Application> Applications { get; set; }

        [Required]
        public int PolicyId { get; set; }

        public Policy Policy { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class FormQuestion
    {
        public int id { get; set; }

        public string question { get; set; }

        public string answer { get; set; }

        //Foreign Key
        public int jobId { get; set; }
        public Job job { get; set; }

        //Foreign Key
        public int answerTypeId { get; set; }
        public AnswerType AnswerType { get; set; }
    }

    public class CandidateRecommender
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

    public class AnswerType
    {
        public int Id { get; set; }
        public string Type { get; set; }
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

    public class JobPlatformHelper
    {

    }

    public class JobApplicationPhase
    {

    }

    public class AppUser
    {

    }

    public class JobPhaseHelpers
    {

    }

    public class Application
    {
        public int Id { get; set; }
        public DateTime TimeApplied { get; set; }
        public DateTime BeginApplication { get; set; }
        public int JobId { get; set; }
        public IList<Job> Job { get; set; }
        public int AppUserId { get; set; }
        public IList<AppUser> AppUser { get; set; }

        public int ApplicationPhaseId { get; set; }

        public ApplicationPhase ApplicationPhase { get; set; }
        public IList<JobPhaseHelpers> PhaseHelpers { get; set; }
    }

    public class WorkHistory : ICloneable
    {
        public int Id { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string JobTitle { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public int AppUserId { get; set; }
        public IList<AppUser> AppUser { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            WorkHistory test = (WorkHistory)obj;
            return string.Equals(this.CompanyName, test.CompanyName, StringComparison.OrdinalIgnoreCase) && string.Equals(this.JobTitle, test.JobTitle, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Description, test.Description, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Education : ICloneable
    {
        public int Id { get; set; }
        [Required]
        public string Insitution { get; set; }
        [Required]
        public string Qualification { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public int AppUserId { get; set; }
        public IList<AppUser> AppUser { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            Education test = (Education)obj;
            return string.Equals(this.Insitution, test.Insitution, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Qualification, test.Qualification, StringComparison.OrdinalIgnoreCase);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class References : ICloneable
    {
        public int Id { get; set; }
        [Required]
        public string RefFirstName { get; set; }
        [Required]
        public string RefLastName { get; set; }
        [Required]
        [Phone]
        public string RefPhone { get; set; }
        [Required]
        [EmailAddress]
        public string RefEmail { get; set; }
        public int AppUserId { get; set; }
        public IList<AppUser> AppUser { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            References test = (References)obj;
            return string.Equals(this.RefFirstName, test.RefFirstName, StringComparison.OrdinalIgnoreCase) && string.Equals(this.RefLastName, test.RefLastName, StringComparison.OrdinalIgnoreCase) && string.Equals(this.RefPhone, test.RefPhone, StringComparison.OrdinalIgnoreCase) && string.Equals(this.RefEmail, test.RefEmail, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ProfilePortfolioLink
    {
        public int Id { get; set; }
        public string GitHubLink { get; set; }
        public string LinkedInLink { get; set; }
        public string TwitterLink { get; set; }
        public string PersonalWebsiteUrl { get; set; }
        public int AppUserId { get; set; }
        public IList<AppUser> AppUser { get; set; }
    }

    public class AdditionalInformation
    {
        public int Id { get; set; }
        [Required]
        public string Disability { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Ethnicity { get; set; }
        [Required]
        public int AppUserId { get; set; }
        public IList<AppUser> AppUser { get; set; }
    }

    public class PersonalInformation
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]

        public string IdNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public int AppUserId { get; set; }
        public IList<AppUser> AppUser { get; set; }
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

    public class SkillBank
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
    public class Status
    {
        public string name { get; set; }
    }

    // This is used to retrieve the skills from an API

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Attribution
    {
        public string name { get; set; }
        public string text { get; set; }
    }

    public class Type
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string infoUrl { get; set; }
        public string name { get; set; }
        public Type type { get; set; }
    }

    public class APIRoot
    {
        public List<Attribution> attributions { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Document
    {
        public int Id { get; set; }

        public string CV { get; set; }

        public string MatricCertificate { get; set; }

        public string UniversityTranscript { get; set; }

        public string AdditionalCert1 { get; set; }

        public string AdditionalCert2 { get; set; }

        public string AdditionalCert3 { get; set; }


        //Foreign Key: AppUser
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
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

    public class ProfilePicture
    {
        public int Id { get; set; }
        public string profilePic { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}