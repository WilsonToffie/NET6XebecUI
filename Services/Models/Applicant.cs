using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class Applicant
    {
        public object Avatar { get; set; }

        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("firstName")] public string FirstName { get; set; }

        [JsonProperty("lastName")] public string LastName { get; set; }

        [JsonProperty("cstMark")] public long CstMark { get; set; }

        [JsonProperty("cstComment")] public string CstComment { get; set; }

        [JsonProperty("interviewRating")] public long InterviewRating { get; set; }

        [JsonProperty("interviewComment")] public string InterviewComment { get; set; }

        [JsonProperty("phase")] public string Phase { get; set; }
    }
}