using Newtonsoft.Json;

namespace XebecPortal.UI.Services.Models
{
    public class AppUser
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("Name")] public string Name { get; set; }

        [JsonProperty("Surname")] public string Surname { get; set; }

        [JsonProperty("Email")] public string Email { get; set; }

        [JsonProperty("Password")] public string Password { get; set; }

        [JsonProperty("Role")] public string Role { get; set; }

        [JsonProperty("Key")] public string Key { get; set; }

        [JsonProperty("Registered")] public bool Registered { get; set; }

        [JsonProperty("LinkVisits")] public long LinkVisits { get; set; }

        public string Image { get; set; }
    }
}