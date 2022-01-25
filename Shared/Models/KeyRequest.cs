using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Shared.Models
{
    public class KeyRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Key { get; set; }
        public string Link { get; set; }
    }
}
