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
}