using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Shared
{
    public class JobState
    {
        public bool Open { get; set; }
        public bool Closed { get; set; }
        public bool Draft { get; set; }
        public bool Filled { get; set; }
        public bool OnHold { get; set; }
    }
}
