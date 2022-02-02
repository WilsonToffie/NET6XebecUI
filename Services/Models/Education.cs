using System;
using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class Education
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("insitution")]
        public string Insitution { get; set; }

        [JsonProperty("qualification")]
        public string Qualification { get; set; }

        [JsonProperty("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTimeOffset EndDate { get; set; }

        [JsonProperty("appUserId")]
        public int AppUserId { get; set; }

        [JsonProperty("appUser")]
        public AppUser AppUser { get; set; }
    }
}