using System;
using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class ApplicationPhaseHelper
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("applicationId")] 
        public int ApplicationId { get; set; }

        [JsonProperty("application")] 
        public Application Application { get; set; }
        [JsonProperty("applicationPhaseId")] 
        public int ApplicationPhaseId { get; set; }

        [JsonProperty("applicationPhase")] 
        public AppPhase ApplicationPhase { get; set; }

        [JsonProperty("statusId")] 
        public int StatusId { get; set; }
        //Todo
        [JsonProperty("status")] public AppStatus Status { get; set; }

        [JsonProperty("timeMoved")] public DateTime TimeMoved { get; set; }

        [JsonProperty("comments")] public string Comments { get; set; }

        [JsonProperty("rating")] public float  Rating { get; set; }
        
        //Todo remove
        public int AppUserId { get; set; }
    }
}