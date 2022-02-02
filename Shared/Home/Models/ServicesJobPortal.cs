using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Shared.Home.Models
{
    public class ServicesJobPortal
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }
        public string DateAdvertised { get; set; }
        public string ApplyByDate { get; set; }
    }
}
