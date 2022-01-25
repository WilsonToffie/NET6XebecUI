using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Shared
{
    public class Job
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreationDate { get; set; }

        public double Compensation { get; set; }

        public int MinimumExperience { get; set; }

        public string Department { get; set; }
    }
}
