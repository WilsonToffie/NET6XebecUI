using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Pages.HR
{
    public class CreateAJobState
    {
        public int JobId { get; set; }

        public List<FormQuestion> ChosenQuestions { get; set; }
    }
}
