using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Services.Models
{
    public class ProfilePortfolioLink
    {
        public int Id { get; set; }
        public string GitHubLink { get; set; }
        public string LinkedInLink { get; set; }
        public string TwitterLink { get; set; }
        public string PersonalWebsiteUrl { get; set; }
        public int AppUserId { get; set; }
        public IList<AppUser> AppUser { get; set; }
    }
}
