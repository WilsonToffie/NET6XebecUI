using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class JobModel
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("compensation")]
        public float  Compensation { get; set; }

        [JsonProperty("minimumExperience")]
        public int MinimumExperience { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("dueDate")]
        public DateTimeOffset DueDate { get; set; }

        [JsonProperty("creationDate")]
        public DateTimeOffset CreationDate { get; set; }

        
        //Todo
        [JsonProperty("jobTypes")]
        public object JobTypes { get; set; }

        [JsonProperty("jobPlatforms")]
        public object JobPlatforms { get; set; }

        [JsonProperty("jobPhases")]
        public object JobPhases { get; set; }

        [JsonProperty("applications")]
        public object Applications { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(Description)}: {Description}, {nameof(Company)}: {Company}, {nameof(Compensation)}: {Compensation}, {nameof(MinimumExperience)}: {MinimumExperience}, {nameof(Location)}: {Location}, {nameof(Department)}: {Department}, {nameof(DueDate)}: {DueDate}, {nameof(CreationDate)}: {CreationDate}";
        }
    }
}