using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Shared
{
    public class CustomQuestion
    {
        public int Id { get; set; }

        public string questionDescription { get; set; }

        public int answerTypeId { get; set; }

        public string answerType { get; set; }
    }
}
