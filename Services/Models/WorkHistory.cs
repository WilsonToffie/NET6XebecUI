using System;
using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class WorkHistory
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

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