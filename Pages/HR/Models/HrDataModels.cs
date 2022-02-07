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
        public DateTime DueDate { get; set; }
        public DateTime CreationDate { get; set; }
        public List<JobTypeHelper> JobTypes { get; set; }
        public List<JobPlatformHelper> JobPlatforms { get; set; }
        public List<JobApplicationPhase> JobPhases { get; set; }
        public List<Application> Applications { get; set; }
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

    public class FormQuestion
    {
        public int id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public int jobId { get; set; }
        public string job { get; set; }
        public int answerTypeId { get; set; }
        public string answerType { get; set; }
    }


    public class CustomQuestion
    {
        public int Id { get; set; }
        public string questionDescription { get; set; }
        public int answerTypeId { get; set; }
        public string answerType { get; set; }
    }



    class Developer
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; }
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

    public class Application
    {

    }

    public class MockLocation
    {
        public int id { get; set; }
        public string location { get; set; }
    }

    public class MockDepartment
    {
        public int id { get; set; }
        public string department { get; set; }
    }

    public class MockSocialMedia
    {
        public int id { get; set; }
        public string socialMedia { get; set; }
    }

    public class JobPlatform
    {
        public int id { get; set; }
        public string platformName { get; set; }
    }
}