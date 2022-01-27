using System;
using System.Globalization;

namespace XebecPortal.UI.Services.Models
{
    public class MyJob
    {
        public string Position { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Phase { get; set; }
        public string Status { get; set; }
        public DateTime LastMoved { get; set; }
        public DateTime ApplicationDate { get; set; }

        protected bool Equals(string other)
        {
            
            return Position.ToLower().Contains(other.ToLower()) || Company.ToLower().Contains(other.ToLower()) ||
                   Location.ToLower().Contains(other.ToLower()) || Phase.ToLower().Contains(other.ToLower()) ||
                   Status.ToLower().Contains(other.ToLower()) ||
                   LastMoved.ToString(CultureInfo.CurrentCulture).ToLower().Contains(other.ToLower()) || 
                   ApplicationDate.ToString(CultureInfo.CurrentCulture).ToLower().Contains(other.ToLower());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyJob) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Company, Location, Phase, Status, LastMoved, ApplicationDate);
        }
    }
}