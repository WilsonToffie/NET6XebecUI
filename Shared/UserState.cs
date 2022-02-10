using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Shared
{
    public class UserState
    {
        public int AppUserId { get; set; } = 1;
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
    }
}
