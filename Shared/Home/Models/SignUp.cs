using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Shared.Home.Models
{
    public class SignUp: SignInModel
    {
        public string Role { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

    }
}
