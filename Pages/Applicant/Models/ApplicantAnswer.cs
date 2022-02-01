using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Pages.Applicant.Models
{
    public class ApplicantAnswer
    {
        public int id { get; set; }

        public string applicantAnswer { get; set; }

        public int questionnaireHRFormId { get; set; }

        public string questionnaireHRForm { get; set; }

        public int appUserId { get; set; }

        public string user { get; set; }
    }
}
