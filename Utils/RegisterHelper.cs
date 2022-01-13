using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XebecPortal.UI
{
     public class RegisterHelper
    {
        public int Id { get; set; }
        public DateTime TimeDateOfRegistration { get; set; }                        
        //foreign key
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

    }
}
