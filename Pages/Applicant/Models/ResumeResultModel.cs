namespace XebecPortal.UI.Pages.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class ResumeResultModel
    {
        [JsonProperty("Name")] public string Name { get; set; } = string.Empty;
        [JsonProperty("Email Address")] public string EmailAddress { get; set; }= string.Empty;
        [JsonProperty("College Name")] public string CollegeName { get; set; } = string.Empty;
        [JsonProperty("Degree")] public string Degree { get; set; } = string.Empty;
        [JsonProperty("Designation")] public string Designation { get; set; }= string.Empty;
        [JsonProperty("Companies worked at")] public string CompaniesWorkedAt { get; set; }= string.Empty;
        [JsonProperty("Graduation Year")] public string GraduationYear { get; set; }= string.Empty;
        
        [JsonProperty("Location")] public string Location { get; set; } = string.Empty;
        

        [JsonProperty("Skills")] public List<string> Skills { get; set; } = new List<string>();

        [JsonProperty("Years of Experience")] public string YearsOfExperience { get; set; } = string.Empty;

        [JsonProperty("UNKNOWN")] public List<string> Unknown { get; set; } = new();

        public override string ToString()
        {
            return
                $"{nameof(Name)}: {Name}, {nameof(Location)}: {Location}, {nameof(CompaniesWorkedAt)}: {CompaniesWorkedAt}, {nameof(CollegeName)}: {CollegeName}, {nameof(Degree)}: {Degree}, {nameof(Designation)}: {Designation}, {nameof(EmailAddress)}: {EmailAddress}, {nameof(GraduationYear)}: {GraduationYear}, {nameof(Skills)}: {Skills}, {nameof(YearsOfExperience)}: {YearsOfExperience}, {nameof(Unknown)}: {Unknown}";
        }
    }
}