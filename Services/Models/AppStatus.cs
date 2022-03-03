using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class AppStatus
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("phaseHelpers")]
        public object PhaseHelpers { get; set; }

        public StatusEnum StatusEnum { get; set; }
    }

    public enum StatusEnum
    {
        InProgress, Rejected
    }
}