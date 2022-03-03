using System;
using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class ApplicationModel
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("jobId")] public int JobId { get; set; }

        [JsonProperty("job")] public JobModel Job { get; set; }

        [JsonProperty("appUserId")] public int AppUserId { get; set; }

        [JsonProperty("appUser")] public Applicant AppUser { get; set; }

        [JsonProperty("timeApplied")] public DateTimeOffset TimeApplied { get; set; }

        [JsonProperty("beginApplication")] public DateTime BeginApplication { get; set; }


        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(JobId)}: {JobId}, {nameof(Job)}: {Job}, {nameof(AppUserId)}: {AppUserId}, {nameof(AppUser)}: {AppUser}, {nameof(TimeApplied)}: {TimeApplied}, {nameof(BeginApplication)}: {BeginApplication}";
        }
    }
}