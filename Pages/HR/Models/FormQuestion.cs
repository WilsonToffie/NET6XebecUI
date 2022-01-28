using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Shared
{
    public class FormQuestion
    {
        public int id { get; set; }

        public string question { get; set; }

        public string answer { get; set; }

        public int jobId { get; set; }

        public string job { get; set; }
    }
}
