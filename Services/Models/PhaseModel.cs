using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class PhaseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("emailTemplate")]
        public object EmailTemplate { get; set; }

        [JsonProperty("phaseHelpers")]
        public object PhaseHelpers { get; set; }

        [JsonProperty("jobPhases")]
        public object JobPhases { get; set; }
    }
}