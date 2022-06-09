using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using XebecPortal.UI.Pages.HR;

namespace XebecPortal.UI.Shared.Home.Models
{
    public class JobPortalModels
    {
        public class ApplicationModel
        {
            [JsonProperty("id")] public int Id { get; set; }

            [JsonProperty("jobId")] public int JobId { get; set; }

            [JsonProperty("job")] public Job Job { get; set; }

            [JsonProperty("appUserId")] public int AppUserId { get; set; }

            [JsonProperty("appUser")] public AppUser AppUser { get; set; }

            [JsonProperty("timeApplied")] public DateTimeOffset TimeApplied { get; set; }

            [JsonProperty("beginApplication")] public DateTime BeginApplication { get; set; }

            [JsonProperty("applicationPhaseId")]
            public int ApplicationPhaseId { get; set; }

            [JsonProperty("applicationPhase")]
            public ApplicationPhase ApplicationPhase { get; set; }

            [JsonProperty("phaseHelpers")]
            public ApplicationPhasesHelper applicationPhasesHelper { get; set; }
        }
    }
}
