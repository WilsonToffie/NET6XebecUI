using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XebecPortal.UI
{
    public class LoginResult
    {
        /*newly added*/
        public int Id { get; set; }
        /*newly added*/

        public string Message { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string JwtBearer { get; set; }

        public bool Success { get; set; }
        /*test*/
        public int AppUserId { get; set; }

    }

    public class LoginModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class RegisterModel : LoginModel
    {

        [Required(ErrorMessage = "User Role is required")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
    }
}
