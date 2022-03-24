using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public  class AdditionalInformation
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("disability")]
        public string Disability { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("ethnicity")]
        public string Ethnicity { get; set; }

        [JsonProperty("appUserId")]
        public long AppUserId { get; set; }

        [JsonProperty("appUser")]
        public object AppUser { get; set; }
    }
}