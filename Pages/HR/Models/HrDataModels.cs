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
        public string JobName { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public string SocialMedia { get; set; }
        public DateTime DateAdvertised { get; set; }
        public DateTime DateDue { get; set; }
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
}