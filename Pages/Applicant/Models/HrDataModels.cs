using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public int jobId { get; set; }

        public string job { get; set; }
        public int answerTypeId { get; set; }
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

}