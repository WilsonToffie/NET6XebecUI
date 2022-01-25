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
        public int id { get; set; }
        public string jobName { get; set; }
        public string companyName { get; set; }
        public string location { get; set; }
        public string department { get; set; }
        public string description { get; set; }
        public string socialMedia { get; set; }
        public DateTime dateAdvertised { get; set; }
        public DateTime dateDue { get; set; }
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