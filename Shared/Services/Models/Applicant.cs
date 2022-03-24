using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class Applicant
    {
        public string Avatar { get; set; }

        [JsonProperty("id")] 
        public int Id { get; set; }

        [JsonProperty("firstName")] 
        [Required]
        [StringLength(50, ErrorMessage = "First name is too int.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "last name is too int.")]

        [JsonProperty("lastName")] 
        public string LastName { get; set; }

        [JsonProperty("cstMark")] 
        public int CstMark { get; set; }

        [JsonProperty("cstComment")] 
        public string CstComment { get; set; }

        [JsonProperty("interviewRating")] 
        public int InterviewRating { get; set; }

        [JsonProperty("interviewComment")] 
        public string InterviewComment { get; set; }

        [JsonProperty("phase")] 
        public string Phase { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}, {nameof(CstMark)}: {CstMark}, {nameof(CstComment)}: {CstComment}, {nameof(InterviewRating)}: {InterviewRating}, {nameof(InterviewComment)}: {InterviewComment}, {nameof(Phase)}: {Phase}";
        }
    }
}