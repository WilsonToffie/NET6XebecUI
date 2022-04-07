using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Shared.Home.Models
{
    public class SignInResult
    {     
        public int AppUserId { get; set; }
        public string Message { get; set; } //used to Display a message to the user on the front end
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string jwtBearer { get; set; }
        public bool Success { get; set; }   //Used as a flag to indicate whether everything was a success or not
        public string Avator { get;set;}
       // public string Title { get;set;} // The title will not have values due to the new API parameters / variables

        
        
    }
}
