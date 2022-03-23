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
        public ApplicationPhase ApplicationPhase{ get; set; }

        [JsonProperty("phaseHelpers")]
        public ApplicationPhasesHelper applicationPhasesHelper { get; set; }
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
        public IList<JobTypeHelper> JobTypes { get; set; }
        public IList<JobPlatformHelper> JobPlatforms { get; set; }
        public IList<JobApplicationPhase> JobPhases { get; set; }
        public IList<Application> Applications { get; set; }
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
            return string.Equals(this.CompanyName, test.CompanyName,StringComparison.OrdinalIgnoreCase) && string.Equals(this.JobTitle, test.JobTitle, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Description, test.Description, StringComparison.OrdinalIgnoreCase);
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
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string ContactNum { get; set; }

        public int AppUserId { get; set; }
        public IList<AppUser> AppUser { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            References test = (References)obj;
            return string.Equals(this.ContactNum, test.ContactNum, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Email, test.Email, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Name, test.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Surname, test.Surname, StringComparison.OrdinalIgnoreCase);
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

    public class SkillsInformation
    {
        public int Id { get; set;}
        public string Description { get; set;}
        public int AppUserId { get; set; }
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
}