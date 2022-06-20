using System.ComponentModel.DataAnnotations;

namespace XebecPortal.UI.Shared.Home.Models
{
    public class ContactUsModel
    {

        [Required]
        public string career { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string surname { get; set; }
        [Required]
        public string demo { get; set; }
        [Required]
        public string query { get; set; }


    }
}
