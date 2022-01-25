using System;
using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class Job
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("compensation")] public double Compensation { get; set; }

        [JsonProperty("minimumExperience")] public int MinimumExperience { get; set; }

        [JsonProperty("location")] public string Location { get; set; }

        [JsonProperty("department")] public string Department { get; set; }

        [JsonProperty("dueDate")] public DateTimeOffset DueDate { get; set; }

        [JsonProperty("creationDate")] public DateTimeOffset CreationDate { get; set; }
    }
}