using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Services.Models
{
    public class ApplicantQuestion
    {
        public int Id { get; set; }

        public string Applicantanswer { get; set; }

        public string HRQuestion { get; set; }

        public int HRQuestionId { get; set; }

        public int TypeId { get; set; }
    }
}
