using System;

namespace XebecPortal.UI.Services.Models
{
    public class MyJob
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Phase { get; set; }
        public string Status { get; set; }
        public DateTime LastMoved { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}