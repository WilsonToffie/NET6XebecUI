using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Pages.Applicant.Models
{
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
}
